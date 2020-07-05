using Godot;
using System;

public class RocketLauncherRocket : Node2D
{
	[Export] public int damage;
	[Export] public NodePath DamageAreaCollisionPath;
	[Export] public PackedScene hitParticlesScene;

	private Enemy rocketCollidedEnemy;
	public Node2D hitParticleRoot;

    public override void _Ready()
    {
		hitParticleRoot = hitParticlesScene.Instance() as Node2D;
	}

    private void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy")
		{
			rocketCollidedEnemy = body as Enemy;
			rocketCollidedEnemy.TakeDamage(damage);
		}
		else if (body.GetType().Name == "BigEnemy")
        {
			rocketCollidedEnemy = body as BigEnemy;
			rocketCollidedEnemy.TakeDamage(damage);
        }

		CollisionShape2D damageAreaCollision = GetNode<CollisionShape2D>(DamageAreaCollisionPath);

		damageAreaCollision.SetDeferred("disabled", false);
		if (hitParticlesScene != null)
		{
			hitParticleRoot.GetNode<Particles2D>("HitParticle").Emitting = true;
			hitParticleRoot.GlobalPosition = GetNode<BulletComponent>("BulletComponent").GlobalPosition;
			if (hitParticleRoot.GetParent() != GetTree().CurrentScene)
				GetTree().CurrentScene.AddChild(hitParticleRoot);

			GetTree().CreateTimer(0.1f).Connect("timeout", this, "OnHitParticleTimerTimeout");
		}

		GetTree().CurrentScene.GetNode<CameraShake>("MainCam").Shake(180, 90, 80);
		GetTree().CreateTimer(0.1f).Connect("timeout", this, "OnTimerTimeout");
	}

	private void OnDamageAreaBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy" && body != rocketCollidedEnemy)
		{
			Enemy enemy = body as Enemy;
			enemy.TakeDamage(damage * 0.5f);
		}
		else if (body.GetType().Name == "BigEnemy" && body != rocketCollidedEnemy)
        {
			BigEnemy enemy = body as BigEnemy;
			enemy.TakeDamage(damage * 0.5f);
        }
	}

	private void OnTimerTimeout() => QueueFree();

	private void OnHitParticleTimerTimeout() => hitParticleRoot.QueueFree();
}