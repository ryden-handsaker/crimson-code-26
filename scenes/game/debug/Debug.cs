using Godot;
using System;
using CrimsonCode26.scenes.game.data;
using System.Collections.Generic;

public partial class Debug : Node2D
{
	private Dictionary<Guid, Machine> parsedMachines;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		/*
		string json = """
				  {
					"2df0450e-6839-44a6-b976-db91370d1bd7": {
					  "data": {
						"path": "~/Downloads/filefactorytest/"
					  },
					  "outputs": {
						"Output": "9d55b047-9002-4c5f-81f6-63c46dc1c41f"
					  },
					  "type": "FolderSource"
					},
					"9d55b047-9002-4c5f-81f6-63c46dc1c41f": {
					  "data": {
						"path": "~/Downloads/filefactorytest/fft"
					  },
					  "type": "FolderDestination"
					}
				  }
""";
*/
		string json = """
					{
					  "1d000e90-3b4c-44ae-81ce-d418c70fa9b1": {
						"data": { "Filetype": [".txt"] },
						"outputs": {
						  "Fail" : "00000000-0000-0000-0000-000000000000",
						  "Match": "e703e9a0-6c4f-45bd-ba93-ed73018e23e5"
						},
						"type": "ExtensionFilter"
					  },
					  "cc2f5955-d88c-47e0-b621-a1d01731a54a": {
						"data": { "path": "~/Downloads/filefactorytest/" },
						"outputs": {"Output": "1d000e90-3b4c-44ae-81ce-d418c70fa9b1"},
						"type": "FolderSource"
					  },
					  "e703e9a0-6c4f-45bd-ba93-ed73018e23e5": {
						"data": { "path": "~/Downloads/filefactorytest/fft" },
						"type": "FolderDestination"
					  }
					}
""";
		GD.Print($"Start of Debug.cs _Ready");
		parsedMachines = MachineParser.ParseJSON(json);
		GD.Print($"End of Debug.cs _Ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (var (guid, machine) in parsedMachines)
		{
			GD.Print($"Hey it's machine {machine.Name}");
			machine.Tick();
		}
	}
}
