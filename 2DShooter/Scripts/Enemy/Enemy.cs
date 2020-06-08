using Godot;
using System;

public class Enemy : RigidBody2D
{
    [Export] public NodePath playerPath;
    [Export] public NodePath detectionPath;
    [Export] public NodePath gunPath;

    [Export] public int hp;
    [Export] public float accel;

    public Area2D detection;
    public Player player;
    public Gun gun;

    private bool iSeePlayer;

    public override void _EnterTree()
    {
        detection = GetNode<Area2D>(detectionPath);
        player = GetNode<Player>(playerPath);
        gun = GetNode<Gun>(gunPath);

        detection.Connect("body_entered", this, "CanSeePlayer");
        detection.Connect("body_exited", this, "CantSeePlayer");
        Connect("body_entered", this, "EnemyCollided");
    }

    public override void _Process(float delta)
    {
        if (hp <= 0)
        {
            QueueFree();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (iSeePlayer && player != null)
        {
            Position = Position.LinearInterpolate(player.Position, accel * delta);
        }
    }

    private void CanSeePlayer(object body)
    {
        if (body.GetType().Name == "Player")
            iSeePlayer = true;
    }

    private void CantSeePlayer(object body)
    {
        if (body.GetType().Name == "Player")
            iSeePlayer = false;
    }

    private void EnemyCollided(object body)
    {
        if (body.GetType().Name == "Bullet")
        {
            hp -= gun.hitPoints;
            GD.Print(hp);
        }
    }
}