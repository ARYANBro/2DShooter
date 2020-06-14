using Godot;
using System;

public class PlayerBullet : Bullet
{
    [Export] public int damage = 20;

    private void OnBulletBodyEntered(object body)
    {
        if (body.GetType().Name == "Enemy")
        {
            Enemy enemy = (Enemy)body;
            enemy.TakeDamage(damage);
        }

        PackedScene BulletParticles = ResourceLoader.Load<PackedScene>("res://Assets/Effects/Player Bullet Particles.tscn");
        Node2D bulletParticles = (Node2D)BulletParticles.Instance();

        AddChild(bulletParticles);

        QueueFree();
    }
}
