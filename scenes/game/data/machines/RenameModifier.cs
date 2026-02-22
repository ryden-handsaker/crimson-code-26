using System;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace CrimsonCode26.scenes.game.data.machines;

public class RenameModifier : Machine, ISerializable<RenameModifier>
{
    public string InputFormat;
    public string OutputFormat;
    
    public override void Process(File file)
    {
        ProcessFile = file;
        Regex regex = new Regex(InputFormat);
        
        // Define a MatchEvaluator to handle the output format
        Match match = regex.Match(file.Name);
        string newName = string.Format(OutputFormat, match.Groups);

        file.Name = newName;
        
        Push(Outputs["Output"]);
    }

    public RenameModifier(Guid guid, string inputFormat, string outputFormat)
    {
        Initialize("Rename Modifier", guid);
        InputFormat = inputFormat;
        OutputFormat = outputFormat;
    }

    public static RenameModifier CreateFromJSON(Guid guid, JsonObject json)
    {
        string inputFormat = json["input_format"]?.GetValue<string>();
        string outputFormat = json["output_format"]?.GetValue<string>();
        return new RenameModifier(guid, inputFormat, outputFormat);
    }
}