using Godot;
using System;

public class PistolBullet : Node2D
{
    [Export] public int damage = 20;
    [Export] public PackedScene hitParticlesScene;

    Particles2D hitParticle;

    public override void _Ready()
    {
        hitParticle = hitParticlesScene.Instance() as Particles2D;
    }

    public void OnBulletBodyEntered(object body)
    {
        if (body is Enemy enemy)
            enemy.TakeDamage(damage);

        hitParticle.Position = GetNode<BulletComponent>("BulletComponent").GlobalPosition;
        hitParticle.Emitting = true;

        GetParent().AddChild(hitParticle);
        QueueFree();
    }
}
