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

    [Export]
    public MachineOption[] Options = [];
}
