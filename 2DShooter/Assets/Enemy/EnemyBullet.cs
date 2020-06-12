using System;
using Godot;

public class EnemyBullet : RigidBody2D
{
    [Export] public float speed = 500.0f;
    [Export] public int damage = 10;

    public override void _Ready()
    {
        LinearVelocity = -Transform.y * speed;
    }

    private void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }

    private void OnEnemyBulletBodyEntered(object body)
    {
        if (body.GetType().Name == "Player")
        {
            Player player = GetTree().CurrentScene.GetNode<Player>("Player");

            if (player != null)
            {
                player.takeDamage(damage);
            }
        }
    }
}