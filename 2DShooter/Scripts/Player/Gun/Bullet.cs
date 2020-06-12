using System;
using Godot;

public class Bullet : RigidBody2D
{
    [Export] public float speed = 500.0f;
    [Export] public int damage;

    public override void _Ready()
    {
        LinearVelocity = -Transform.y * speed;
    }

    private void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }
}