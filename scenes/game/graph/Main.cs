using Godot;
using CrimsonCode26.scripts;
using System;
using System.Linq;

namespace CrimsonCode26.scenes.game.graph;

public partial class Main : Control
{

	[Export]
	private PackedScene _machineScene;
	
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
		var inputNode = _graphEdit.GetNode<MachineTemplate>(new NodePath(fromNode));
		
		_graphEdit.DisconnectNode(fromNode, fromPort, toNode, toPort);

		inputNode.SetOutputConnection(Guid.Empty, fromPort);
	}

	private void OnDeleteNodesRequest(StringName[] nodes)
	{
		foreach (var item in nodes)
		{
			string nodeName = item.ToString();

			// Remove all connections w/ this node before deleting it
			foreach (var c in _graphEdit.GetConnectionList()
				.Where(c => (string)c["from_node"] == nodeName || (string)c["to_node"] == nodeName)
				.ToList())
			{
				_graphEdit.DisconnectNode(
					(string)c["from_node"], (int)(long)c["from_port"],
					(string)c["to_node"],   (int)(long)c["to_port"]);
			}

			var node = _graphEdit.GetNode<GraphNode>(nodeName);
			node.QueueFree();
		}
	}

	private void AddMachine(MachineResource resource)
	{
		var machineNode = _machineScene.Instantiate<MachineTemplate>();
		
		machineNode.Initialize(resource);
		
		_graphEdit.AddChild(machineNode);
		machineNode.PositionOffset = new Vector2(100, 100);
	}

	private void OnButtonButtonDown()
	{
		var json = _graphEdit.ToJSON();
		
		var packedScene = ResourceLoader.Load<PackedScene>("uid://sydgop38y2sb");
		var visualizer = packedScene.Instantiate<Visualizer>();
		visualizer.JsonMap = json;

		GetTree().Root.AddChild(visualizer);
		GetTree().CurrentScene.QueueFree();
		GetTree().CurrentScene = visualizer;
	}

	private void OnSaveButtonPressed()
	{
		var json = _graphEdit.ToJSON();

		GD.Print(json);
	}
}
