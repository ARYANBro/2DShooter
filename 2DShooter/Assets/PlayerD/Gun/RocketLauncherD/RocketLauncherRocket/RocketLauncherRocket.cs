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
			rocketCollidedEnemy = body as Enemy;
			rocketCollidedEnemy.TakeDamage(damage);
		}

		CollisionShape2D damageAreaCollision = GetNode<CollisionShape2D>(DamageAreaCollisionPath);
		damageAreaCollision.SetDeferred("disabled", false);
		if (hitParticlesScene != null)
		{
			Node2D hitParticleRoot = hitParticlesScene.Instance() as Node2D;
			hitParticleRoot.GetNode<Particles2D>("HitParticle").Emitting = true;
			hitParticleRoot.GlobalPosition = GetNode<BulletComponent>("BulletComponent").GlobalPosition;
			GetTree().Root.AddChild(hitParticleRoot);
		}

		GetTree().CurrentScene.GetNode<CameraShake>("MainCam").Shake(100, 80, 80);
		GetTree().CreateTimer(0.1f).Connect("timeout", this, "OnTimerTimeout");
	}

	private void OnDamageAreaBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy" && body != rocketCollidedEnemy)
		{
			Enemy enemy = body as Enemy;
			enemy.TakeDamage(damage / 1.5f);
		}
	}

	private void OnTimerTimeout() => QueueFree();
}