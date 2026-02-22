using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Nodes;
using CrimsonCode26.scenes.game.data;

namespace CrimsonCode26.scenes.game.data.machines;

public class ExtensionFilter : Machine, ISerializable<ExtensionFilter>
{
    private List<string> _extensions = [];
    
    public IReadOnlyList<string> Extensions => _extensions;
    
    public void SetExtensions(IEnumerable<string> extensions)
    {
        _extensions = extensions?.ToList() ?? [];
    }
    
    public override void Process(File file)
    {
        if (ProcessFile != null) throw new InvalidAsynchronousStateException(); // eh
        
        ProcessFile = file;
        if (Extensions.Any(extension => file.Extension.Equals(extension)))
        {
            Push(Outputs["Pass"]);
        }
        else
        {
            Push(Outputs["Fail"]);
        }
    }

    public ExtensionFilter(Guid guid)
    {
        this.Initialize("Extension Filter", guid);
    }

    public static ExtensionFilter CreateFromJSON(Guid guid, JsonObject json)
    {
        var machine = new ExtensionFilter(guid);

        if (json.TryGetPropertyValue("extensions", out var extensionNode) && extensionNode is JsonArray extensions)
            machine.SetExtensions(extensions.Select(extenstion => extenstion?.GetValue<string>() ?? string.Empty)
                .Where(str => !string.IsNullOrWhiteSpace(str)));
        
        return machine;
    }
}