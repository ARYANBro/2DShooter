using Godot;
using System;

public class Enemy : RigidBody2D
{
    /* Paths */
    [Export] public NodePath enemyDetectionPath;
    [Export] public NodePath DeathParticlesPath;
    [Export] public NodePath animPlayerPath;
    [Export] public NodePath deathTimerPath;
    [Export] public NodePath playerPath;
    [Export] public NodePath healthPath;

    /* Movement */
    [Export] public float acceleration = 1f;
    [Export] public int health = 100;

    /* Ref */
    public Particles2D DeathParticles;
    public AnimationPlayer animPlayer;
    public KinematicBody2D player;
    public Area2D healthDetection;
    public Area2D enemyDetection;
    public Timer deathTimer;

    /* Var */
    private bool iSeePlayer = false;

    public override void _EnterTree()
    {
        DeathParticles = GetNode<Particles2D>(DeathParticlesPath);
        animPlayer = GetNode<AnimationPlayer>(animPlayerPath);
        enemyDetection = GetNode<Area2D>(enemyDetectionPath);
        healthDetection = GetNode<Area2D>(healthPath);
        player = GetNode<KinematicBody2D>(playerPath);
        deathTimer = GetNode<Timer>(deathTimerPath);

        enemyDetection.Connect("body_exited", this, "CantSeePlayer");
        enemyDetection.Connect("body_entered", this, "CanSeePlayer");
        healthDetection.Connect("body_entered", this, "EnemyHit");
        deathTimer.Connect("timeout", this, "DeathTimerTimeOut");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (player != null)
        {
            if (iSeePlayer)
                Position = Position.LinearInterpolate(player.Position, acceleration * delta);
        }
    }

    /* Can Can't See player */
    private void CanSeePlayer(object body)
    {
        if (player != null)
        {
            if (body.GetType().Name == "Player")
                iSeePlayer = true;
        }
    }

    private void CantSeePlayer(object body)
    {
        if (player != null)
        {
            if (body.GetType().Name == "Player")
                iSeePlayer = false;
        }
    }
    /* Enemy Hit */
    private void EnemyHit(object body)
    {
        if (body.GetType().Name == "Bullet")
        {
            health -= 20;

            if (health <= 0)
            {
                DeathParticles.Emitting = true;
                animPlayer.Play("Death_animation");
                deathTimer.Start();
            }
        }
    }

    private void DeathTimerTimeOut()
    {
        DeathParticles.QueueFree();
        enemyDetection.QueueFree();
        QueueFree();
    }
}