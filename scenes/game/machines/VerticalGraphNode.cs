using Godot;

[GlobalClass]
public partial class VerticalGraphNode : GraphNode
{
    // public override void _DrawPort(int slotIndex, Vector2I position, bool left, Color color)
    // {
    //     if (left)
    //     {
    //         DrawCircle(new Vector2(Size.X / 2, 0), 10, color);
    //     }
    //     else
    //     {
    //         DrawCircle(new Vector2(Size.X / 2, Size.Y), 10, color);
    //     }
    // }

    public Vector2 GetVerticalInputPosition(int port)
    {
        return Position + new Vector2(Size.X / 2, 0);
    }

    public Vector2 GetVerticalOutputPosition(int port)
    {
        return Position + new Vector2(Size.X / 2, Size.Y);
    }

    public Rect2 GetInputHotzone(int port)
    {
        return new Rect2(GetVerticalInputPosition(port) + new Vector2(-10, -10), new Vector2(20, 20));
    }

    public Rect2 GetOutputHotzone(int port)
    {
        return new Rect2(GetVerticalOutputPosition(port) + new Vector2(-10, -10), new Vector2(20, 20));
    }
}
