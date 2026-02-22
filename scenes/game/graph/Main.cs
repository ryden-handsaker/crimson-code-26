using Godot;
using CrimsonCode26.scripts;

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
		_graphEdit.ConnectNode(fromNode, fromPort, toNode, toPort);
		
		var outputNode = _graphEdit.GetNode<MachineTemplate>(new NodePath(toNode));
		var inputNode = _graphEdit.GetNode<MachineTemplate>(new NodePath(fromNode));
		
		inputNode.SetOutputConnection(outputNode.Guid, fromPort);
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
