using Godot;
using Godot.Collections;
using CrimsonCode26.scripts;

namespace CrimsonCode26.scenes.game.graph;

public partial class GraphEdit : Godot.GraphEdit
{
	public string ToJSON()
	{
		var dict = new Dictionary<string, Variant>();
		
		foreach (var child in GetChildren())
		{
			if (child is not MachineTemplate machine)
				continue;
			
			var machineData = new Dictionary<string, Variant>
			{
				{ "type", machine.Type.ToString() }
			};

			if (machine.Resource.Outputs.Length > 0)
			{
				var outputs = machine.Resource.Outputs;
				var outputDict = new Dictionary<string, string>();
					
				for (var i = 0; i < outputs.Length; i++)
					outputDict.Add(outputs[i], machine.OutputConnections[i].ToString());
				
				machineData.Add("outputs", outputDict);
			}

			if (machine.Resource.Options.Length > 0)
			{
				var options = machine.Resource.Options;
				var optionDict = new Dictionary<string, Variant>();
				
				foreach (var option in options)
				{
					var value = machine.GetOptionValue(option.Key);

					if (option.IsList)
					{
						Array result = [];

						foreach (var token in value.Split(','))
							result.Add(ParseValue(option.Type, token.Trim()));
						
						optionDict.Add(option.Key, result);
					}
					else
						optionDict.Add(option.Key, ParseValue(option.Type, value));

				}
				
				machineData.Add("data", optionDict);
			}
			
			dict.Add(machine.Guid.ToString(), machineData);
		}

		return Json.Stringify(dict);
	}

	public static Variant ParseValue(MachineOption.OptionType type, string value)
	{
		return type switch
		{
			MachineOption.OptionType.Integer => int.Parse(value),
			MachineOption.OptionType.Float => float.Parse(value),
			_ => value
		};
	}
}
