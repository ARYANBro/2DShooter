using Godot;
using System;
using System.IO.Ports;

public class PlayerBullet : Bullet
{
	[Export] 
	public int damage = 20;
	[Export]
	public PackedScene hitParticlesScene;

	private Particles2D hitParticle;

	public override void _Ready()
	{
		hitParticle = (Particles2D)hitParticlesScene.Instance();
		LinearVelocity = -Transform.y * speed;
	}

	private void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy")
		{
			Enemy enemy = (Enemy)body;
			enemy.TakeDamage(damage);
		}

		hitParticle.Position = Position;
		GetTree().CurrentScene.AddChild(hitParticle);
		hitParticle.Emitting = true;

		// Delete the hit particle.
		// Todo.

		QueueFree();
	}
}
