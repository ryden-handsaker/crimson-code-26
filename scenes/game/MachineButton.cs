using Godot;
using System;
using CrimsonCode26.scripts;

namespace CrimsonCode26.scenes.game;

public partial class MachineButton : Button
{
	[Signal]
	public delegate void MachineButtonPressedEventHandler(MachineResource machineResource);
	
	[Export]
	private MachineResource _resource;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = _resource.Type.ToString();
	}

	private void OnPressed()
	{
		EmitSignal(SignalName.MachineButtonPressed, _resource);
	}
}
