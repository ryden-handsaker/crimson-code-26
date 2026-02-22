using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CrimsonCode26.scenes.game.data;
using CrimsonCode26.scripts;
using Godot;

namespace CrimsonCode26.scenes.game;

public partial class MachineTemplate : GraphNode
{
	public Guid Guid { get; protected set; }
	public MachineParser.Type Type { get; protected set; }
	public MachineResource Resource { get; protected set; } // used in the JSON stringifier so it knows what the machine be
	
	private Guid[] _outputConnections;
	public ImmutableList<Guid> OutputConnections => _outputConnections.ToImmutableList();

	private readonly Dictionary<string, LineEdit> _optionInputs = new();

	private int _outputPort; // keeps track of the output port mapping (after input and options node ports)

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void Initialize(MachineResource machine)
	{
		Guid = Guid.NewGuid();
		Type = machine.Type;

		_outputPort = machine.Inputs.Length + machine.Options.Length;

		_outputConnections = new Guid[machine.Outputs.Length];
		
		Resource = machine;
		
		Title = machine.Type.ToString();

		if (machine.Inputs.First() is var input)
			AddInput(input); // there can only be one input max, so we just do that lowk

		foreach (var option in machine.Options)
		{
			if (option.Type == MachineResource.OptionType.Path)
				AddFileOption(option.Label);
			else
				AddOption(option.Label);
		}
		
		foreach (var output in machine.Outputs)
			AddOutput(output);
	}

	private void AddInput(string label)
	{
        var inputRow = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Text = label
        };
        
        AddChild(inputRow);
        
		SetSlotEnabledLeft(0, true);
		SetSlotTypeLeft(0, 0);
		SetSlotColorLeft(0, new Color(1, 1, 1, 1));
	}

	private void AddOutput(string label)
	{
        var outputRow = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            Text = label
        }; 
        
        AddChild(outputRow);
        
		SetSlotEnabledRight(_outputPort, true);
		SetSlotTypeRight(_outputPort, 0);
		SetSlotColorRight(_outputPort, new Color(1, 0, 0, 1));
		
		_outputPort++;
	}

	private void AddOption(string label)
	{
		var hbox = new HBoxContainer();
		
		var text = new Label	
		{
			HorizontalAlignment = HorizontalAlignment.Left,
			Text = label + ":"
		};
		
		var textEdit = new LineEdit
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		
		_optionInputs[label] = textEdit;
		
		hbox.AddChild(text);
		hbox.AddChild(textEdit);
		
		AddChild(hbox);
	}
	
	private void AddFileOption(string label)
	{
		var hbox = new HBoxContainer();
		
		var text = new Label	
		{
			HorizontalAlignment = HorizontalAlignment.Left,
			Text = label + ":"
		};
		
		var textEdit = new LineEdit
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};

		_optionInputs[label] = textEdit;

		var browseButton = new Button { Icon = GetThemeIcon("Folder", "EditorIcons") };

		var fileDialog = new FileDialog
		{
			FileMode = FileDialog.FileModeEnum.OpenFile,
			Access = FileDialog.AccessEnum.Filesystem
		};

		browseButton.Pressed += () => fileDialog.PopupCentered();
		
		fileDialog.FileSelected += (path) => textEdit.Text = path;
		
		hbox.AddChild(text);
		hbox.AddChild(textEdit);
		hbox.AddChild(browseButton);
		
		AddChild(hbox);
		AddChild(fileDialog);
	}

	public void SetOutputConnection(Guid output, int slotId)
	{
		_outputConnections[slotId - Resource.Inputs.Length + Resource.Options.Length] = output;
	}

	public string GetOptionValue(string label)
	{
		return _optionInputs.TryGetValue(label, out var input)
			? input.Text
			: throw new KeyNotFoundException($"option {label} not found");
	}
}
