using Godot;
using Godot.Collections;
using System;

public partial class Main : HBoxContainer
{

	private Dictionary<String, Variant> machineDict = [];

	[Export] PackedScene machineScene;
	[Export] MachineResource testResource;
	GraphEdit graphEdit;
	VBoxContainer buttons;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		graphEdit = GetNode<GraphEdit>("%GraphEdit");
		buttons = GetNode<VBoxContainer>("%Buttons");

		foreach (MachineButton button in buttons.GetChildren())
		{
			button.MachineButtonPressed += AddMachine;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnConnectionRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
	{
		graphEdit.ConnectNode(fromNode, fromPort, toNode, toPort);
	}

	private void AddMachine(MachineResource resource)
	{
		var machineNode = machineScene.Instantiate<MachineTemplate>();
		machineNode.Initialize(resource);
		graphEdit.AddChild(machineNode);
	}
}
