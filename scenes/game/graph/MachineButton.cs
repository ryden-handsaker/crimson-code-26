using Godot;
using System;

public partial class MachineButton : Button
{
	[Signal]
	public delegate void MachineButtonPressedEventHandler(MachineResource machineResource);
	[Export] MachineResource machineResource;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = machineResource.Type;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPressed()
	{
		EmitSignal(SignalName.MachineButtonPressed, machineResource);
	}
}
