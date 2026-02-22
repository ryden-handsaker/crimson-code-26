using Godot;
using CrimsonCode26.scenes.game.data;

namespace CrimsonCode26.scripts;

[GlobalClass]
public partial class MachineResource : Resource
{
    [Export]
    public string[] Inputs = [];

    [Export]
    public string[] Outputs = [];

    [Export]
    public MachineParser.Type Type { get; set; }

    public enum OptionType
    {
        Integer, Float, Path, Text
    }

    [GlobalClass]
    public partial class MachineOption : Resource
    {
        [Export]
        public string Label { get; set; }
        
        [Export]
        public MachineResource.OptionType Type { get; set; }
        
        [Export]
        public bool IsList { get; set; }
    }

    [Export]
    public MachineOption[] Options = [];
}
