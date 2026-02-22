using Godot;

namespace CrimsonCode26.scripts;

[GlobalClass]
public partial class MachineOption : Resource
{
    [Export]
    public string Label { get; set; }
    
    [Export]
    public string Key { get; set; }
        
    public enum OptionType
    {
        Integer, Float, Path, Text
    }
    
    [Export]
    public OptionType Type { get; set; }
        
    [Export]
    public bool IsList { get; set; }
}