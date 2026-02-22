using System;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace CrimsonCode26.scenes.game.data.machines;

public class RenameModifier : Machine, ISerializable<RenameModifier>
{
    public string InputFormat { get; private set; }
    public string OutputFormat { get; private set; }
    public override void Process(File file)
    {
        ProcessFile = file;
        Regex regex = new Regex(InputFormat);
        
        
        // Define a MatchEvaluator to handle the output format
        Match match = regex.Match(file.Name);
        string newName = string.Format(OutputFormat, match.Groups);
        
        // TODO: actually rename the file

        // Perform the replacement and return the result
        
        Push(Outputs["Output"]);
    }

    public RenameModifier(string inputFormat, string outputFormat)
    {
        InputFormat = inputFormat;
        OutputFormat = outputFormat;
    }

    public static RenameModifier CreateFromJSON(Guid guid, JsonObject json)
    {
        throw new NotImplementedException();
    }
}