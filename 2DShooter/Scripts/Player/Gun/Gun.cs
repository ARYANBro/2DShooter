using System;
using Godot;

public class Gun : KinematicBody2D {
    [Export] public float fireSpeed = 500.0f;
    public Position2D firepoint;
    public PackedScene bulletScene;
    private Vector2 mousePos;

    public override void _Ready() {
        bulletScene = (PackedScene) ResourceLoader.Load("res://Assets/Player/Gun/Bullet.tscn");
        firepoint = GetNode<Position2D>("Firepoint");
        GetTree().CurrentScene.Connect("OnPlayerDied", this, "PlayerDied");
    }

    public override void _Process(float delta) {
        mousePos = GetGlobalMousePosition();
    }

    public override void _PhysicsProcess(float delta) {
        Vector2 lookDir = mousePos - GlobalPosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);
        RotationDegrees = Mathf.Rad2Deg(angle) + 90;

        if (Input.IsActionJustPressed("Shoot"))
            Shoot(lookDir);
    }

    private void Shoot(Vector2 lookDir) {
        RigidBody2D bullet = (RigidBody2D) bulletScene.Instance();
        bullet.Position = firepoint.GlobalPosition;
        bullet.Rotation = GlobalRotation;
        GetTree().Root.AddChild(bullet);
    }

    private void OnPlayerDied() {
        QueueFree();
    }
}