using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Nodes;

namespace CrimsonCode26.scenes.game.data.machines;

public class ExtensionFilter : Machine, ISerializable<ExtensionFilter>
{
    private List<string> _extensions = [];
    
    public ImmutableList<string> Extensions => _extensions.ToImmutableList();
    
    private void SetExtensions(IEnumerable<string> extensions)
    {
        _extensions = extensions?.ToList() ?? [];
        //foreach (var extension in _extensions)
        //    Godot.GD.Print($"{extension}");
    }
    
    public override void Process(File file)
    {
        if (ProcessFile != null)
            throw new InvalidAsynchronousStateException(); // eh
        
        ProcessFile = file;
        
        Push(Extensions.Any(extension => file.Extension.Equals(extension)) ? Outputs["Match"] : Outputs["Fail"]);
    }

    public ExtensionFilter(Guid guid)
    {
        Initialize(guid);
    }

    public static ExtensionFilter CreateFromJSON(Guid guid, JsonObject json)
    {
        var machine = new ExtensionFilter(guid);
        // json.TryGetPropertyValue("extensions", out var myNode);
        // Godot.GD.Print($"Node: {myNode}");
        if (json.TryGetPropertyValue("extensions", out var extensionNode) && extensionNode is JsonArray extensions)
            machine.SetExtensions(
                extensions
                    .Select(extenstion => extenstion?.GetValue<string>() ?? string.Empty)
                    .Where(str => !string.IsNullOrWhiteSpace(str))
            );
        
        return machine;
    }

    public new void Tick()
    {
        Godot.GD.Print("ExtensionFilter Tick");
    }
}