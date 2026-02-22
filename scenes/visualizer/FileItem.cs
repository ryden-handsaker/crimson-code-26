using Godot;
using System;
using System.Net;

public partial class FileItem : PathFollow2D
{
	[Export] public string fileName = "";
	public double lifespan = 1.0f;
	// Called when the node enters the scene tree for the first time.
	async public override void _Ready()
	{
		GetNode<Label>("%Label").Text = fileName;
		var tween = CreateTween();

		tween.TweenProperty(
			this,
			"progress_ratio",
			1.0f,
			lifespan
		);

		await ToSignal(tween, Tween.SignalName.Finished);

    	this.QueueFree();
	}
}
