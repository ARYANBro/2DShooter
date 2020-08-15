using Godot;

public class Utlities
{
    public static RandomNumberGenerator random = new RandomNumberGenerator();
    public static float LookAtMouse(Vector2 mousePosition, Vector2 nodePosition)
    {
        Vector2 lookDir = mousePosition - nodePosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);
        return Mathf.Rad2Deg(angle) + 90;
    }

    public static float LookAtSomething(Vector2 nodeToLookAtPosition, Vector2 nodePosition)
    {
        Vector2 lookDir = nodeToLookAtPosition - nodePosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);
        return Mathf.Rad2Deg(angle) + 90;
    }

    public static Vector2 RandPosition(Vector2 globalPosition, SceneTree sceneTree)
    {
        Vector2 resolution = sceneTree.Root.GetVisibleRect().Size;

        random.Randomize();
        return globalPosition = new Vector2(random.RandfRange(0f, resolution.x),
                           random.RandfRange(0f, resolution.y));
    }


    public static Vector2 RandPosition(ref Node2D node, SceneTree sceneTree)
    {
        Vector2 resolution = sceneTree.Root.GetVisibleRect().Size;

        random.Randomize();
        return node.GlobalPosition = new Vector2(random.RandfRange(0f, resolution.x),
                                    random.RandfRange(0f, resolution.y));
    }

    public static void AddChildWithParams(Node parent, Node2D node, Vector2 globalPos, float rotDegrees)
    {
        node.GlobalPosition = globalPos;
        node.RotationDegrees = rotDegrees;
        parent.AddChild(node);
    }
    public static void SetNode2DParams<T>(ref T node, Vector2 globalPos, float rotDegrees) where T : Node2D
    {
        if (!node.IsInsideTree()) node.Position = globalPos;
        else node.GlobalPosition = globalPos;

        node.RotationDegrees = rotDegrees;
    }
}
