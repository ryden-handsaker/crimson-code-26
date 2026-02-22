using System.Xml;
using Godot;

public partial class MachineTemplate : GraphNode
{
	// [Export] MachineResource machine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// if (machine.Inputs.Length != 0) AddInput(machine.Inputs[0]); // there can only be one input max

		// for (int i = 0; i < machine.Options.Length; i++)
		// {
		// 	AddOption(machine.Options[i]);
		// }
		// for (int i = 0; i < machine.Outputs.Length; i++)
		// {
		// 	AddOutput(machine.Outputs[i], i + 1 + machine.Options.Length);
		// }
	}

	public void Initialize(MachineResource machine)
	{
		if (machine.Inputs.Length != 0) AddInput(machine.Inputs[0]); // there can only be one input max

		for (int i = 0; i < machine.Options.Length; i++)
		{
			AddOption(machine.Options[i]);
		}
		for (int i = 0; i < machine.Outputs.Length; i++)
		{
			AddOutput(machine.Outputs[i], i + machine.Inputs.Length + machine.Options.Length);
		}

		Title = machine.Type;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

	private void AddOutput(string label, int id)
	{
        var outputRow = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            Text = label
        };
        AddChild(outputRow);
		SetSlotEnabledRight(id, true);
		SetSlotTypeRight(id, 0);
		SetSlotColorRight(id, new Color(1, 0, 0, 1));
	}

	private void AddOption(string label)
	{
		var hbox = new HBoxContainer();
		var text = new Label
		{
			HorizontalAlignment = HorizontalAlignment.Left,
			Text = label + ":"
		};
		var textEdit = new TextEdit
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
			
		};
		hbox.AddChild(text);
		hbox.AddChild(textEdit);
		AddChild(hbox);
	}
}
