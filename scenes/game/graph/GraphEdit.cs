using Godot;
using Godot.Collections;
using System;

public partial class GraphEdit : Godot.GraphEdit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void toJSON()
	{
		var dict = new Dictionary<string, Variant>();
		foreach (MachineTemplate machine in GetChildren())
		{
			var machineData = new Dictionary<string, Variant>();
			machineData.Add("type", machine.Type);

			if (machine.Resource.Outputs.Length > 0)
			{
				var outputs = machine.Resource.Outputs;
				var outputDict = new Dictionary<string, string>();
				
				for (int i = 0; i < outputs.Length; i++)
				{
					outputDict.Add(outputs[i], machine.OutputConnections[i]);
				}
				machineData.Add("outputs", outputs);
			}
		
			dict.Add()
		}
	}
}
