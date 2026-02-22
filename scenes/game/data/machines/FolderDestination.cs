using System;
using System.ComponentModel;
using System.Text.Json.Nodes;

namespace CrimsonCode26.scenes.game.data.machines;

public class FolderDestination : Machine, ISerializable<FolderDestination>
{
	public string Path { get; protected set; }

	public override void Process(File file)
	{
		if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh

		string newPath = System.IO.Path.Combine([
			Path,
			string.Concat([file.Name, file.Extension])
		]);

		try
		{
			System.IO.File.Move(
				file.Path,
				newPath
			);
		}
		catch (System.IO.IOException e)
		{
			Godot.GD.Print($"Move from {file.Path} to {newPath} failed");
			Godot.GD.Print($"It's likely that {file.Name}{file.Extension} already exists at {Path}");
		}
	}

	public FolderDestination(Guid guid)
	{
		Initialize(guid);
	}
	
	public static FolderDestination CreateFromJSON(Guid guid, JsonObject json)
	{
		var machine = new FolderDestination(guid);
		
		if (json["path"]?.GetValue<string>() is var path)
			machine.Path = File.TildeToHome(path);

		return machine;
	}
}
