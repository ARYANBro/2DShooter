using Godot;
using System;

public class PlayerBullet : Bullet
{
	[Export] 
	public int damage = 20;
	[Export]
	public PackedScene hitParticlesScene;
	public Timer hitParticleLifetime;

	private Particles2D hitParticle;

	public override void _Ready()
	{
		hitParticle = (Particles2D)hitParticlesScene.Instance();
		hitParticleLifetime = GetParent().GetNode<Timer>("HitParticleLifetime");
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
		hitParticle.Emitting = true;
		hitParticleLifetime.Start();

		// Delete the hit particle #TODO
		/******************************/
		// hitParticle.QueueFree();

		GetTree().CurrentScene.AddChild(hitParticle);
		QueueFree();
	}
	private void OnParticleLifetimeEnd()
	{
		hitParticle.QueueFree();
		if (hitParticle.IsQueuedForDeletion())
        {
			GD.Print("Deleted hitparticle");
        }
	}
}
