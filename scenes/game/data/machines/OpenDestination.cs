using System;
using System.ComponentModel;
using System.Text.Json.Nodes;

namespace CrimsonCode26.scenes.game.data.machines;

public class OpenDestination : Machine, ISerializable<OpenDestination>
{
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh

        Godot.GD.Print($"Opening: {file.Name}");
        var process = System.Diagnostics.Process.Start(file.Path);
        if (process == null)
        {
            Godot.GD.PrintErr($"No default app associated with: {file.Name}");
        }
    }

    public OpenDestination(Guid guid)
    {
        Initialize(guid);
    }

    public static OpenDestination CreateFromJSON(Guid guid, JsonObject json)
    {
        return new OpenDestination(guid);
    }
}