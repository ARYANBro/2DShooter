using Godot;
using System;

public class Gun : KinematicBody2D
{
    [Export] public NodePath cameraPath;

    public Camera2D camera;

    private Vector2 mousePos;

    public override void _EnterTree()
    {
        camera = GetNode<Camera2D>(cameraPath);
    }

    public override void _Process(float delta)
    {
        mousePos = camera.GetGlobalMousePosition();
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 lookdir = mousePos - Position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) + 90;

        RotationDegrees = Mathf.Rad2Deg(angle);
    }

}
