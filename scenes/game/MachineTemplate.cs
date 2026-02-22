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

	public void Initialize(MachineResource machine)
	{
		Title = MachineParser.GetName(machine.Type);
		
		Guid = Guid.NewGuid();
		Type = machine.Type;

		_outputPort = machine.Inputs.Length + machine.Options.Length;

		_outputConnections = new Guid[machine.Outputs.Length];
		
		Resource = machine;

		if (machine.Inputs.Length > 0)
			AddInput(machine.Inputs.First()); // there can only be one input max, so we just do that lowk

		foreach (var option in machine.Options)
		{
			if (option.Type == MachineOption.OptionType.Path)
				AddFileOption(option.Label, option.Key);
			else
				AddOption(option.Label, option.Key);
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

	private void AddOption(string label, string key)
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
		
		_optionInputs[key] = textEdit;
		
		hbox.AddChild(text);
		hbox.AddChild(textEdit);
		
		AddChild(hbox);
	}
	
	private void AddFileOption(string label, string key)
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

		_optionInputs[key] = textEdit;

		var browseButton = new Button { Icon = GD.Load<Texture2D>("res://assets/FolderBrowse.svg") };

		var fileDialog = new FileDialog
		{
			FileMode = FileDialog.FileModeEnum.OpenDir,
			Access = FileDialog.AccessEnum.Filesystem
		};

		browseButton.Pressed += () => fileDialog.PopupCentered();
		
		fileDialog.DirSelected += (path) => textEdit.Text = path;
		
		hbox.AddChild(text);
		hbox.AddChild(textEdit);
		hbox.AddChild(browseButton);
		
		AddChild(hbox);
		AddChild(fileDialog);
	}

	public void SetOutputConnection(Guid output, int slotId)
	{
		//GD.Print(slotId - Resource.Inputs.Length + Resource.Options.Length - 1);
		_outputConnections[slotId - Resource.Inputs.Length + Resource.Options.Length - 1] = output;
	}

	public string GetOptionValue(string key)
	{
		return _optionInputs.TryGetValue(key, out var input)
			? input.Text
			: throw new KeyNotFoundException($"option '{key}' not found");
	}
}
