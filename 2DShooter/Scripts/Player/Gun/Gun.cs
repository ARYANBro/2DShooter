using Godot;
using System;

public class Gun : KinematicBody2D {
    [Export] public PackedScene bulletScene;
    [Export] public NodePath camPath;
    [Export] public float fireSpeed;
    [Export] public int hitPoints;

    public Position2D firepoint;
    private Vector2 mousePos;
    private Vector2 lookDir;
    public Camera2D cam;

    public override void _Ready() {
        firepoint = GetNode<Position2D>("Firepoint");
        cam = GetNode<Camera2D>(camPath);
    }

    public override void _Process(float delta) {
        mousePos = cam.GetGlobalMousePosition();
    }

    public override void _PhysicsProcess(float delta) {
        lookDir = mousePos - GlobalPosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);

        RotationDegrees = Mathf.Rad2Deg(angle) + 90;

        if (Input.IsActionJustPressed("Shoot"))
            Shoot();
    }

    private void Shoot() {
        RigidBody2D bullet = (RigidBody2D)bulletScene.Instance();
        bullet.Position = firepoint.GlobalPosition;
        bullet.Rotation = GlobalRotation;
        bullet.AddForce(new Vector2(), lookDir.Normalized() * fireSpeed);

        GetTree().Root.AddChild(bullet);
    }
}