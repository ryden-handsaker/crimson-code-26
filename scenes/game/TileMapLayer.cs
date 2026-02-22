using Godot;
using System;

public partial class TileMapLayer : Godot.TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PlaceMachine(Vector2I pos)
	{
		SetCell(pos, 0, new Vector2I(0, 0));
	}
}
