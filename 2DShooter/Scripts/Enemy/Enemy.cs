using Godot;
using System;

public class Enemy : RigidBody2D
{
    [Export] public NodePath playerPath;
    [Export] public NodePath detectionPath;
    [Export] public NodePath gunPath;

    [Export] public float stopAccel;
    [Export] public float linearVel;
    [Export] public float speed;
    [Export] public int hp;

    public Area2D detection;
    public Player player;
    public Gun gun;

    private bool iSeePlayer;
    private Vector2 velocity;

    public override void _Ready() {
        detection = GetNode<Area2D>(detectionPath);
        player = GetNode<Player>(playerPath);
        gun = GetNode<Gun>(gunPath);

        detection.Connect("body_exited", this, "PlayerNotSeen");
        detection.Connect("body_entered", this, "PlayerSeen");
        Connect("body_entered", this, "EnemyCollided");
    }

    public override void _Process(float delta) {
        if (hp <= 0) {
            QueueFree();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (iSeePlayer && player != null) {
            Vector2 forcePos = player.Position - Position;
            LinearVelocity = forcePos.Normalized() * linearVel;
        } else if (!iSeePlayer && player != null)
            LinearVelocity = LinearVelocity.LinearInterpolate(new Vector2(), delta * stopAccel);
    }

    public void PlayerSeen(object body) {
        if (body.GetType().Name == "Player")
            iSeePlayer = true;
    }

    public void PlayerNotSeen(object body) {
        if (body.GetType().Name == "Player")
            iSeePlayer = false;
    }

    public void EnemyCollided(object body) {
        if (body.GetType().Name == "Bullet")
            hp -= gun.hitPoints;
    }
}