using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class VerticalGraphEdit : GraphEdit
{
    public override void _Ready()
    {
        base._Ready();
        //ConnectionRequest += OnConnectionRequest;
        //DisconnectionRequest += OnDisconnectionRequest;
    }

    override public Vector2[] _GetConnectionLine(Vector2 fromPosition, Vector2 toPosition)
    {
        bool fromFound = false;
        bool toFound = false;

        List<Vector2> line = [];

        foreach (Node child in GetChildren())
        {
            if (child is VerticalGraphNode node)
            {
                for (int port = 0; port < node.GetInputPortCount(); port++)
                {
                    if ((node.Position + node.GetInputPortPosition(port)).IsEqualApprox(toPosition))
                    {
                        line.Add(node.GetVerticalInputPosition(port));
                        toFound = true;
                        break;
                    }
                }

                for (int port = 0; port < node.GetOutputPortCount(); port++)
                {
                    if ((node.Position + node.GetOutputPortPosition(port)).IsEqualApprox(fromPosition))
                    {
                        line.Add(node.GetVerticalOutputPosition(port));
                        fromFound = true;
                        break;
                    }
                }
            }
        }

        if (!fromFound)
            line.Add(fromPosition);

        if (!toFound)
            line.Add(toPosition);

        return line.ToArray();
    }

    // Optional: tweak this for the "radius" of the hotzone
    private Vector2I DefaultPortSize = new(8, 8);

    public override bool _IsInInputHotzone(GodotObject inNode, int inPort, Vector2 mousePosition)
    {
        if (inNode is GraphNode graphNode)
        {
            // Get the port position (GraphNode method)
            Vector2 portPos = graphNode.GetInputPortPosition(inPort) * Zoom + graphNode.Position;

            // Convert back for the check, like the C++ code
            Vector2 scaledPos = portPos / Zoom;

            // Check if mouse is in the port hotzone
            return IsInPortHotzone(scaledPos, mousePosition, DefaultPortSize, true);
        }

        return false;
    }
    public override bool _IsInOutputHotzone(GodotObject outNode, int outPort, Vector2 mousePosition)
    {
        if (outNode is GraphNode graphNode)
        {
            // Get the port position (GraphNode method)
            Vector2 portPos = graphNode.GetOutputPortPosition(outPort) * Zoom + graphNode.Position;

            // Convert back for the check, like the C++ code
            Vector2 scaledPos = portPos / Zoom;

            // Check if mouse is in the port hotzone
            return IsInPortHotzone(scaledPos, mousePosition, DefaultPortSize, true);
        }

        return false;
    }
    private bool IsInPortHotzone(Vector2 portPos, Vector2 mousePos, Vector2I portSize, bool isInputPort)
    {
        // Make a rectangle around the port position
        Rect2 hotzone = new Rect2(portPos - new Vector2(portSize.X, portSize.Y) * 0.5f, portSize);

        return hotzone.HasPoint(mousePos);
    }

    private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
    {
        ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
    }

    private void OnDisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
    {
        DisconnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
    }
}
