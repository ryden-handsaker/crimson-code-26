using Godot;
using System;

public partial class FileItem : PathFollow2D
{
	[Export] protected string fileName = "";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProgressRatio = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ProgressRatio += (float)(delta / 10);
	}
}
