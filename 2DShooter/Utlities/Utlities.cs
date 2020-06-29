using Godot;
using System;

    public class Utlities : Node
    {
        public static float LookAtMouse(Vector2 mousePosition, Vector2 nodePosition)
        {
            Vector2 lookDir = mousePosition - nodePosition;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x);
            return Mathf.Rad2Deg(angle) + 90;
        }
    }
