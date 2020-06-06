using Godot;
using System;

public class Bullet : RigidBody2D
{
    /* Paths */
    [Export] public NodePath bulletAnimationsPath;
    [Export] public NodePath bullletAnimTimerPath;
    [Export] public NodePath bulletAreaPath;
    [Export] public NodePath particlesPath;
    [Export] public NodePath lifetimePath;

    /* Movemnet */
    [Export] public float bulletSpeed = 1000.0f;

    /* Ref */
    public AnimationPlayer bulletAnimations;
    public Particles2D particles;
    public Timer bulletAnimTimer;
    public Area2D bulletArea;
    public Timer lifetime;

    public override void _EnterTree()
    {   
        bulletAnimations = GetNode<AnimationPlayer>(bulletAnimationsPath);
        bulletAnimTimer = GetNode<Timer>(bullletAnimTimerPath);
        particles = GetNode<Particles2D>(particlesPath);
        bulletArea = GetNode<Area2D>(bulletAreaPath);
        lifetime = GetNode<Timer>(lifetimePath);
    
        bulletAnimTimer.Connect("timeout", this, "BulletAnimTimerEnd");
        bulletArea.Connect("body_entered", this, "BulletCollided");
        lifetime.Connect("timeout", this, "LifetimeEnd");
    }

    private void BulletCollided(object body)
    {
        lifetime.Start();
        particles.Emitting = true;
    }

    public void Fire(Vector2 lookDir)
    {
        lookDir = lookDir.Normalized();
        AddForce(new Vector2(0.0f, 0.0f), ((180 / Mathf.Pi) * lookDir) * bulletSpeed);
    }

    private void LifetimeEnd()
    {
        bulletAnimations.Play("Bullet Die");
        bulletAnimTimer.Start();
    }
}