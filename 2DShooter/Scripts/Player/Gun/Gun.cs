using Godot;
using System;

public class Gun : KinematicBody2D
{
    [Export] public PackedScene bulletScene;
    [Export] public NodePath camPath;
    [Export] public NodePath firePointPath;

    [Export] public float fireSpeed;
    [Export] public int hitPoints;

    public Position2D firePoint;
    public Camera2D cam;

    private Vector2 mousePos;
    private Vector2 lookdir;

    public override void _EnterTree()
    {
        firePoint = GetNode<Position2D>(firePointPath);
        cam = GetNode<Camera2D>(camPath);
    }

    public override void _Process(float delta)
    {
        mousePos = cam.GetGlobalMousePosition();
    }

    public override void _PhysicsProcess(float delta)
    {
        lookdir = mousePos - GlobalPosition;
        float angle = Mathf.Atan2(lookdir.y, lookdir.x);

        RotationDegrees = Mathf.Rad2Deg(angle) + 90;

        if (Input.IsActionJustPressed("Shoot"))
            Shoot();
    }

    private void Shoot()
    {
        Bullet bullet = (Bullet)bulletScene.Instance();
        bullet.Position = firePoint.GlobalPosition;
        bullet.Rotation = GlobalRotation;

        bullet.Fire(lookdir, fireSpeed);

        GetTree().Root.AddChild(bullet);
    }
}