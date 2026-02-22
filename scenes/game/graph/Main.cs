using Godot;
using CrimsonCode26.scripts;
using System;
using System.Linq;

namespace CrimsonCode26.scenes.game.graph;

public partial class Main : HBoxContainer
{

	[Export]
	private PackedScene _machineScene;
	
	[Export]
	private MachineResource _testResource;
	
	private GraphEdit _graphEdit;
	private VBoxContainer _buttons;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_graphEdit = GetNode<GraphEdit>("%GraphEdit");
		_buttons = GetNode<VBoxContainer>("%Buttons");

		foreach (var node in _buttons.GetChildren())
		{
			var button = (MachineButton)node;
			button.MachineButtonPressed += AddMachine;
		}
	}

	private void OnConnectionRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
	{
		var outputNode = _graphEdit.GetNode<MachineTemplate>(new NodePath(toNode));
		var inputNode = _graphEdit.GetNode<MachineTemplate>(new NodePath(fromNode));

		// only allow 1 connection max
		bool outputOccupied = _graphEdit.GetConnectionList().Any(c =>
			(string)c["from_node"] == fromNode &&
			(long)c["from_port"] == fromPort);

		bool inputOccupied = _graphEdit.GetConnectionList().Any(c =>
			(string)c["to_node"] == toNode &&
			(long)c["to_port"] == toPort);

		if (outputOccupied || inputOccupied)
		{
            var dialog = new AcceptDialog
            {
                DialogText = "Only 1 connection allowed per input/output!"
            };
            AddChild(dialog);
			dialog.PopupCentered();
			return;
		}

		_graphEdit.ConnectNode(fromNode, fromPort, toNode, toPort);
		
		
		inputNode.SetOutputConnection(outputNode.Guid, fromPort);
	}

	private void OnDisconnectRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
	{
		var outputNode = _graphEdit.GetNode<MachineTemplate>(new NodePath(toNode));
		var inputNode = _graphEdit.GetNode<MachineTemplate>(new NodePath(fromNode));
		
		_graphEdit.DisconnectNode(fromNode, fromPort, toNode, toPort);

		inputNode.SetOutputConnection(Guid.Empty, fromPort);
	}

	private void AddMachine(MachineResource resource)
	{
		var machineNode = _machineScene.Instantiate<MachineTemplate>();
		
		machineNode.Initialize(resource);
		
		_graphEdit.AddChild(machineNode);
	}

	private void OnButtonButtonDown()
	{
		var json = _graphEdit.ToJSON();
		
		var packedScene = ResourceLoader.Load<PackedScene>("uid://sydgop38y2sb");
		var visualizer = packedScene.Instantiate<Visualizer>();
		visualizer.JsonMap = json;

		GD.Print(json);

		GetTree().Root.AddChild(visualizer);
		GetTree().CurrentScene.QueueFree();
		GetTree().CurrentScene = visualizer;
	}
}
