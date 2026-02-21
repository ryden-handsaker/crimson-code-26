using Godot;
using System;

public partial class GraphNode : Godot.GraphNode
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GD.Print("hi");
		//SetSlot(0, true, 0, new Color(1, 1, 1, 1), true, 0, new Color(0, 1, 1, 1));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
