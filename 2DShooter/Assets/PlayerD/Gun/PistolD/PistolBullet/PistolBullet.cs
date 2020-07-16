using Godot;
using System;

public class PistolBullet : Node2D
{
	[Export] public int damage = 20;
	[Export] public PackedScene hitParticlesScene;

	private Particles2D hitParticle;

	public override void _Ready()
	{
		hitParticle = hitParticlesScene.Instance() as Particles2D;
	}
	
	public void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy")
		{
			Enemy enemy = body as Enemy;
			enemy.TakeDamage(damage);
		}
		else if (body.GetType().Name == "BigEnemy")
		{
			BigEnemy enemy = body as BigEnemy;
			enemy.TakeDamage(damage);
		}

		hitParticle.Position = GetNode<BulletComponent>("BulletComponent").GlobalPosition;
		hitParticle.Emitting = true;

		GetParent().AddChild(hitParticle);
		QueueFree();
	}
}
