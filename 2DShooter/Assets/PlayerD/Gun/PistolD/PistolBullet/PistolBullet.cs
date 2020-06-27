using Godot;
using System;

public class PistolBullet : Node2D
{
	[Export] 
	public int damage = 20;
	[Export]
	public PackedScene hitParticlesScene;

	private Particles2D hitParticle;

	public override void _Ready() => hitParticle = (Particles2D)hitParticlesScene.Instance();
	
	public void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy")
		{
			Enemy enemy = (Enemy)body;
			enemy.TakeDamage(damage);
		}

		Vector2 bulletCompPos = GetNode<BulletComponent>("BulletComponent").GlobalPosition;
		hitParticle.Position = bulletCompPos;
		GetParent().AddChild(hitParticle);
		hitParticle.Emitting = true;
		QueueFree();
	}
}
