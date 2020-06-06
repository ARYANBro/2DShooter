using Godot;
using System;

public class Enemy : RigidBody2D
{
    /* Paths */
    [Export] public NodePath enemyDetectionPath;
    [Export] public NodePath playerPath;

    /* Movement */
    [Export] public float acceleration = 1f;
    [Export] public int health = 100;

    /* Ref */
    public KinematicBody2D player;
    public Area2D enemyDetection;

    /* Var */
    private bool iSeePlayer = false;

    public override void _EnterTree()
    {
        enemyDetection = GetNode<Area2D>(enemyDetectionPath);
        player = GetNode<KinematicBody2D>(playerPath);

        enemyDetection.Connect("body_exited", this, "CantSeePlayer");
        enemyDetection.Connect("body_entered", this, "CanSeePlayer");
        Connect("body_entered", this, "EnemyCollided");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (iSeePlayer)
            Position = Position.LinearInterpolate(player.Position, acceleration * delta);
    }

    /* Can Can't See player */
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

    /* Testing  Collision */
    private void EnemyCollided(object body)
    {
        GD.Print(body.GetType().Name);
    }
}
