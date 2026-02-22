using System;
using System.Collections.Generic;
using System.Linq;
using CrimsonCode26.scenes.game.data;
using CrimsonCode26.scenes.game.data.machines;
using Godot;

public partial class Visualizer : Node2D
{
    [Export(PropertyHint.MultilineText)] public string JsonMap;
	[Export] public PackedScene ItemScene;
	private Dictionary<Guid, Machine> ParsedMachines;
	
	private TileMapLayer _belts;
	private TileMapLayer _machines;
	
	
	private Control _nameContainer;
	private Node2D _pathContainer;

	private Dictionary<Guid, Path2D> _paths = new();

	//private var conveyorStraightDirections;

    public override void _Ready()
	{
		_belts = GetNode<TileMapLayer>("%Belts");
		_machines = GetNode<TileMapLayer>("%Machines");
		
		_nameContainer = GetNode<Control>("%MachineNames");
		_pathContainer = GetNode<Node2D>("%Paths");
		
		Visualize();
	}

	public void Visualize()
	{
		ParsedMachines = MachineParser.ParseJSON(JsonMap);

		// first find source
		var source = FindFileSource();

		// draw source at top left
		DrawMachineAndOutputs(source, new Vector2I(2, 2));
	}

	private void AddMachineAndOutputs(List<Machine> updateOrder, Machine machine)
	{
        updateOrder.Add(machine);
		foreach (Machine machineOut in machine.Outputs.Values)
		{
			AddMachineAndOutputs(updateOrder, machineOut);
		}
	}

	private Stack<Machine> GetUpdateOrder()
	{
		List<Machine> updateOrder = new();
		foreach (Machine machine in ParsedMachines.Values)
		{
			if (machine is FolderSource)
			{
				AddMachineAndOutputs(updateOrder, machine);
			}
		}

		return new Stack<Machine>(updateOrder);
	}

	private void Tick()
	{
		// create reversed update order
		Stack<Machine> updateOrder = GetUpdateOrder();

		while (updateOrder.Count > 0)
		{
			var machine = updateOrder.Pop();
			machine.Tick();
		}

		foreach (Machine machine in ParsedMachines.Values)
		{
			if (machine.GetBeltState().Count > 0)
			{
				var item = ItemScene.Instantiate<FileItem>();
				var file = machine.GetBeltState().Peek();
				item.fileName = file.Name + file.Extension;
				_paths[machine.Id].AddChild(item);
			}
		}
	}

	

	private FolderSource FindFileSource()
	{
		foreach (Machine machine in ParsedMachines.Values)
		{
			if (machine is FolderSource folderSource) return folderSource;
		}

		throw new ArgumentException("Can't find file source!");
	}

	// returns how far right ts function got
	private int DrawMachineAndOutputs(Machine machine, Vector2I pos)
	{
		var conveyorInputOffset = new Vector2I(-1, 0);
		var rightConveyorOutputOffset = new Vector2I(1, 0);
		var bottomConveyorOutputOffset = new Vector2I(0, 2);
		var rightMachineOffset = new Vector2I(4, 0);
		var bottomMachineOffset = new Vector2I(3, 4);

		for (var y = 0; y < 3; y++)
		{
			for (var x = 0; x < 3; x++)
			{
				var worldOffset = new Vector2I(x - 1, y - 1);

				var atlasCoords = new Vector2I(x, y + 2);

				_machines.SetCell(pos + worldOffset, 0, atlasCoords);
			}
		}

		var label = new Label
		{
			Text = Machine.GetName(machine.GetType()),
			Position = new Vector2(pos.X * 32 - 28, pos.Y * 32 - 50),
			HorizontalAlignment = HorizontalAlignment.Center,
			Modulate = new Color(0, 0, 0, 1),
		};
		_nameContainer.AddChild(label);
		
		int failPathDistance = 0;
		int passPathDistance = 0;

		// depth first search, going down the fail path first
		foreach((string outputName, Machine nextMachine) in machine.Outputs)
		{
			if (!StringComparer.OrdinalIgnoreCase.Equals(outputName, "fail")) continue;

			failPathDistance = DrawMachineAndOutputs(nextMachine, pos + bottomMachineOffset);
			failPathDistance += bottomMachineOffset.X;
			DrawConveyorBelts(pos + bottomConveyorOutputOffset, pos + bottomMachineOffset + conveyorInputOffset, nextMachine.Id);
			break;
		}

		// then go down other path
		foreach((string outputName, Machine nextMachine) in machine.Outputs)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(outputName, "fail")) continue;

			var offset = new Vector2I(failPathDistance, 0); // offset x to make room for the fail path
			passPathDistance = DrawMachineAndOutputs(nextMachine, pos + offset + rightMachineOffset);
			passPathDistance += rightMachineOffset.X;
			DrawConveyorBelts(pos + rightConveyorOutputOffset, pos + offset + rightMachineOffset + conveyorInputOffset, nextMachine.Id);
		}
		return failPathDistance + passPathDistance;
	}

	private void DrawConveyorBelts(Vector2I startPos, Vector2I endPos, Guid machineGuid)
	{
		var path = new Path2D();
		var curve = new Curve2D();

		curve.AddPoint(startPos * 32);
		curve.AddPoint(startPos with { Y = endPos.Y } * 32);
		curve.AddPoint(endPos * 32 + new Vector2I(32, 0));
		path.Curve = curve;

		_pathContainer.AddChild(path);
		_paths.Add(machineGuid, path);

		const int rotateRight = (int)TileSetAtlasSource.TransformTranspose | (int)TileSetAtlasSource.TransformFlipH;
		const int rotate180 = (int)TileSetAtlasSource.TransformTranspose;
		for (int y = startPos.Y; y < endPos.Y; y++) // conveyor belts should only ever go from top left to bottom right
		{
			_belts.SetCell(startPos with { Y = y }, 0, new Vector2I(0, 0));
		}
		if (startPos.Y != endPos.Y) // this means there should be a conveyor turn
		{
			_belts.SetCell(startPos with { Y = endPos.Y }, 0, new Vector2I(0, 1), rotate180);
		}
		for (int x = startPos.X + 1; x < endPos.X; x++)
		{
			_belts.SetCell(endPos with { X = x }, 0, new Vector2I(0, 0), rotateRight);
		}
	}

	private void OnTickButtonPressed()
	{
		Tick();
	}
}