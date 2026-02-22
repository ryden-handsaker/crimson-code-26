using Godot;

[GlobalClass]
public partial class MachineResource : Resource
{
    [Export]
    public string[] Inputs;

    [Export]
    public string[] Outputs;

    [Export]
    public string Type;

    [Export]
    public string[] Options;
}
