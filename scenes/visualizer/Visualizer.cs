using System;
using System.Collections.Generic;
using CrimsonCode26.scenes.game.data;
using CrimsonCode26.scenes.game.data.machines;
using Godot;

public partial class Visualizer : Node2D
{
    [Export(PropertyHint.MultilineText)] public string JsonMap;
	private Dictionary<Guid, Machine> ParsedMachines;
	private TileMapLayer _tileMap;

    public override void _Ready()
	{
		_tileMap = GetNode<TileMapLayer>("%TileMapLayer");
		Visualize();
	}

	public void Visualize()
	{
		ParsedMachines = MachineParser.ParseJSON(JsonMap);

		// first find source
		var source = FindFileSource();

		// draw source at top left
		DrawMachineAndOutputs(source, new Vector2I(0, 0));

		// find outputs of source
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
		_tileMap.SetCell(pos, 0, new Vector2I(0, 0));

		int failPathDistance = 0;
		int passPathDistance = 0;

		// depth first search, going down the fail path first
		foreach((string outputName, Machine nextMachine) in machine.Outputs)
		{
			if (!StringComparer.OrdinalIgnoreCase.Equals(outputName, "fail")) continue;

			failPathDistance = DrawMachineAndOutputs(nextMachine, pos + new Vector2I(2, 4));
			break;
		}

		foreach((string outputName, Machine nextMachine) in machine.Outputs)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(outputName, "fail")) continue;

			passPathDistance = DrawMachineAndOutputs(nextMachine, pos + new Vector2I(4, 0));
		}
		return failPathDistance + passPathDistance;
	}
}