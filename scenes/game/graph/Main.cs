using Godot;
using System;

public partial class Main : HBoxContainer
{

	GraphEdit graphEdit;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		graphEdit = GetNode<GraphEdit>("%GraphEdit");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnConnectionRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
	{
		graphEdit.ConnectNode(fromNode, fromPort, toNode, toPort);
	}
}
