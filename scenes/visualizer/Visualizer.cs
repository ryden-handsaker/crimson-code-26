using System;
using System.Collections.Generic;
using System.Linq;
using CrimsonCode26.scenes.game.data;
using CrimsonCode26.scenes.game.data.machines;
using Godot;

public partial class Visualizer : Node2D
{
    [Export(PropertyHint.MultilineText)] public string JsonMap;
	private Dictionary<Guid, Machine> ParsedMachines;
	private TileMapLayer _tileMap;
	private Control _nameContainer;

	//private var conveyorStraightDirections;

    public override void _Ready()
	{
		_tileMap = GetNode<TileMapLayer>("%TileMapLayer");
		_nameContainer = GetNode<Control>("%MachineNames");
		//_tileMap.GetCellAlternativeTile()
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

	private void Tick()
	{
		foreach (Machine machine in ParsedMachines.Values)
		{
			machine.Tick();
			//GD.Print(machine.)
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

		for (int i = 0; i < 9; i++)
		{
			var offset = new Vector2I(i % 3 - 1, i / 3 - 1);
			_tileMap.SetCell(pos + offset, 1, new Vector2I(0, 0));
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
			DrawConveyorBelts(pos + bottomConveyorOutputOffset, pos + bottomMachineOffset + conveyorInputOffset);
			break;
		}

		// then go down other path
		foreach((string outputName, Machine nextMachine) in machine.Outputs)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(outputName, "fail")) continue;

			var offset = new Vector2I(failPathDistance, 0); // offset x to make room for the fail path
			passPathDistance = DrawMachineAndOutputs(nextMachine, pos + offset + rightMachineOffset);
			passPathDistance += rightMachineOffset.X;
			DrawConveyorBelts(pos + rightConveyorOutputOffset, pos + offset + rightMachineOffset + conveyorInputOffset);
		}
		return failPathDistance + passPathDistance;
	}

	private void DrawConveyorBelts(Vector2I startPos, Vector2I endPos)
	{
		const int rotateRight = (int)TileSetAtlasSource.TransformTranspose | (int)TileSetAtlasSource.TransformFlipH;
		const int rotate180 = (int)TileSetAtlasSource.TransformTranspose;
		for (int y = startPos.Y; y < endPos.Y; y++) // conveyor belts should only ever go from top left to bottom right
		{
			_tileMap.SetCell(startPos with { Y = y }, 0, new Vector2I(0, 0));
		}
		if (startPos.Y != endPos.Y) // this means there should be a conveyor turn
		{
			_tileMap.SetCell(startPos with { Y = endPos.Y }, 0, new Vector2I(0, 1), rotate180);
		}
		for (int x = startPos.X + 1; x < endPos.X; x++)
		{
			_tileMap.SetCell(endPos with { X = x }, 0, new Vector2I(0, 0), rotateRight);
		}
	}
}