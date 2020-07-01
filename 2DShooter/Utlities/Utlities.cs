using Godot;
using System;

public class Utlities : Node
{
    public static RandomNumberGenerator randNumGenerator = new RandomNumberGenerator();
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

    public static Vector2 RandPosition(Vector2 resolution, Vector2 globalPosition)
    {
        randNumGenerator.Randomize();

        return globalPosition = new Vector2(randNumGenerator.RandfRange(0f, resolution.x),
                randNumGenerator.RandfRange(0f, resolution.y));
    }
}
