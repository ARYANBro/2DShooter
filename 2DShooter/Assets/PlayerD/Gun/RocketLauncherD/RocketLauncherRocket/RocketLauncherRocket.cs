using Godot;
using System.Linq.Expressions;
using System.Security.Policy;

public class RocketLauncherRocket : Node2D
{
	[Export] public int damage;
	[Export] public NodePath DamageAreaCollisionPath;
	[Export] public PackedScene hitParticlesScene;

	private Enemy rocketCollidedEnemy;

	private void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy")
		{
			rocketCollidedEnemy = (Enemy)body;
			rocketCollidedEnemy.TakeDamage(damage);
		}
		var DamageAreaCollision = GetNode<CollisionShape2D>(DamageAreaCollisionPath);
		DamageAreaCollision.SetDeferred("disabled", false);
		if (hitParticlesScene != null)
		{
			var hitParticleRoot = (Node2D)hitParticlesScene.Instance();
			hitParticleRoot.GetNode<Particles2D>("HitParticle").Emitting = true;
			var bulletCompPos = GetNode<BulletComponent>("BulletComponent").GlobalPosition;

			hitParticleRoot.GlobalPosition = bulletCompPos;

			GetTree().Root.AddChild(hitParticleRoot);
		}

		CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");
		cameraShake.Shake(100 , 80, 80);
		GetTree().CreateTimer(0.1f).Connect("timeout", this, "OnTimerTimeout");

	}

	private void OnDamageAreaBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy" && body != rocketCollidedEnemy)
		{
			Enemy enemy = (Enemy)body;
			enemy.TakeDamage(damage / 1.5f);
		}
	}

	private void OnTimerTimeout()
	{
		QueueFree();
	}
}
