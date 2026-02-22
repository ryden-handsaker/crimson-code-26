using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using CrimsonCode26.scenes.game.data.machines;

namespace CrimsonCode26.scenes.game.data;

public static class MachineParser
{
	public enum Type
	{
		FolderSource,
		FolderDestination, TrashDestination, DoNothingDestination, OpenDestination,
		ExtensionFilter,
		RenameModifier
	}

	public static readonly Dictionary<Type, Func<Guid, JsonObject, Machine>> Factories = new()
	{
		{ Type.FolderSource, FolderSource.CreateFromJSON },
		{ Type.FolderDestination, FolderDestination.CreateFromJSON },
		{ Type.TrashDestination, TrashDestination.CreateFromJSON },
		{ Type.DoNothingDestination, DoNothingDestination.CreateFromJSON },
		{ Type.OpenDestination, DoNothingDestination.CreateFromJSON },
		{ Type.ExtensionFilter, ExtensionFilter.CreateFromJSON },
		{ Type.RenameModifier, ExtensionFilter.CreateFromJSON }
	};

	public static Dictionary<Guid, Machine> ParseJSON(string input)
	{
		var parsedMachines = new Dictionary<Guid, Machine>();

		var root = JsonNode.Parse(input) as JsonObject
			?? throw new ArgumentException("bad json");

		var machines = new Dictionary<Guid, (MachineParser.Type Type, Dictionary<string, Guid> Outputs, JsonObject Data)>();

		foreach (var entry in root)
		{
			if (!Guid.TryParse(entry.Key, out var guid))
				throw new ArgumentException("key is not valid guid");

			var node = entry.Value as JsonObject
					   ?? throw new ArgumentException("node is not a json object");

			if (!Enum.TryParse(node["type"]?.GetValue<string>(), out MachineParser.Type type))
				throw new ArgumentException("node has invalid type: " + node["type"].GetValue<string>());

			var outputs = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
			if (node.TryGetPropertyValue("outputs", out var outputNode) && outputNode is JsonObject outputObject)
			{
				foreach (var output in outputObject)
				{
					var str = output.Value?.GetValue<string>();
					if (!Guid.TryParse(str, out var outputGuid))
						throw new ArgumentException("output has invalid guid");
					outputs[output.Key] = outputGuid;
				}
			}

			if (node.TryGetPropertyValue("data", out var dataNode) && dataNode is JsonObject dataObject)
			{
				machines[guid] = (type, outputs, dataObject);
			}
			else machines[guid] = (type, outputs, null);
		}

		foreach (var (guid, machine) in machines) 
		{
			if (!Factories.TryGetValue(machine.Type, out var factory))
				throw new ArgumentException("unknown machine type: " + machine.Type);

			parsedMachines[guid] = factory(guid, machine.Data);
		}

		foreach (var (guid, machine) in machines)
		{
			var parsed = parsedMachines[guid];
			
			foreach (var (name, targetGuid) in machine.Outputs)
			{
				if (!parsedMachines.TryGetValue(targetGuid, out var target))
					throw new ArgumentException("invalid reference as output");

				parsed.AddConnection(name, target);
			}
		}

		return parsedMachines;
	}

	public static string GetName(MachineParser.Type type)
	{
		return type switch
		{
			Type.FolderSource => "Folder Source",
			Type.FolderDestination => "Folder Destination",
			Type.ExtensionFilter => "Extension Filter",
			Type.TrashDestination => "Trash",
			Type.DoNothingDestination => "Do Nothing",
			Type.OpenDestination => "Open",
			Type.RenameModifier => "Rename",
			_ => throw new ArgumentException("unknown type")
		};
	}
}
