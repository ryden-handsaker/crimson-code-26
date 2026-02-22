using Godot;
using System;

public partial class Game : Node2D
{

	TileMapLayer tileMap;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tileMap = GetNode<TileMapLayer>("%TileMapLayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector2I cellPos = tileMap.LocalToMap(mousePos);
			tileMap.PlaceMachine(cellPos);
		}
	}
}
