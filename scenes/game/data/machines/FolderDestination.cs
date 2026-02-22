using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Nodes;
using Godot;

namespace CrimsonCode26.scenes.game.data.machines;

public class FolderDestination : Machine, ISerializable<FolderDestination>
{
    public string Path { get; protected set; }
    
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh
        
        if (System.IO.Path.GetDirectoryName(file.Path) is var source)
        {
            if (source != null)
                Directory.Move(source, Path);
        }
    }

    public FolderDestination(Guid guid)
    {
        Initialize("Folder Destination", guid);
    }
    
    public static FolderDestination CreateFromJSON(Guid guid, JsonObject json)
    {
        var machine = new FolderDestination(guid);
        
        if (json["path"]?.GetValue<string>() is var path)
            machine.Path = path;

        return machine;
    }
}