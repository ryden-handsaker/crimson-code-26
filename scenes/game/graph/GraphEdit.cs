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

	public string toJSON()
	{
		var dict = new Dictionary<string, Variant>();
		foreach (Node child in GetChildren())
		{
			if (child is MachineTemplate machine)
			{
				var machineData = new Dictionary<string, Variant>();
				machineData.Add("type", machine.Type);

				if (machine.Resource.Outputs.Length > 0)
				{
					var outputs = machine.Resource.Outputs;
					var outputDict = new Dictionary<string, string>();
					
					for (int i = 0; i < outputs.Length; i++)
					{
						outputDict.Add(outputs[i], machine.OutputConnections[i].ToString());
					}
					machineData.Add("outputs", outputDict);
				}

				if (machine.Resource.Options.Length > 0)
				{
					var options = machine.Resource.Options;
					var optionDict = new Dictionary<string, string[]>();

					for (int i = 0; i < options.Length; i++)
					{
						optionDict.Add(options[i], [".txt"]); // TODO: implement this lil bro
					}
					machineData.Add("data", optionDict);
				}

				machineData.Add("position", machine.Position);
			
				dict.Add(machine.Guid.ToString(), machineData);	
			}
			
		}

		return Json.Stringify(dict);
	}
}
