using Godot;
using System;

public partial class FilterBlock : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnMouseEntered()
	{
		Scale = Scale with {X = 1.1f, Y = 1.1f};
	}

	private void OnMouseExited()
	{
		Scale = Scale with {X = 1.0f, Y = 1.0f};
	}
	
	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			GD.Print("pressed");
		}
	}
}
