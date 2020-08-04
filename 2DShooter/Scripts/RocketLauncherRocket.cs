using Godot;
using System;

public class RocketLauncherRocket : Node2D
{
	[Export] public int damage;
	[Export] public NodePath DamageAreaCollisionPath;
	[Export] public PackedScene hitParticlesScene;

	Enemy rocketCollidedEnemy;
	public Node2D hitParticleRoot;
	bool enalbeSlowMo = false;

    public override void _Ready()
    {
		hitParticleRoot = hitParticlesScene.Instance() as Node2D;
	}

	public override void _Process(float delta)
	{
		if (enalbeSlowMo)
			Engine.TimeScale = 0.5f;

		else Engine.TimeScale = 1.0f;
	}

    async void OnBulletBodyEntered(object body)
	{
		if (body is Enemy enemy)
			enemy.TakeDamage(damage);

		var damageAreaCollision = GetNode<CollisionShape2D>(DamageAreaCollisionPath);

		damageAreaCollision.SetDeferred("disabled", false);
		if (hitParticlesScene != null)
		{
			hitParticleRoot.GetNode<Particles2D>("HitParticle").Emitting = true;
			hitParticleRoot.GlobalPosition = GetNode<BulletComponent>("BulletComponent").GlobalPosition;
			if (hitParticleRoot.GetParent() != GetTree().CurrentScene)
				GetTree().CurrentScene.AddChild(hitParticleRoot);
		}

		GetTree().CurrentScene.GetNode<CameraShake>("MainCam").Shake(180, 90, 80);

		enalbeSlowMo = true;
		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		enalbeSlowMo = false;

		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		QueueFree();
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		hitParticleRoot.QueueFree();
	}

	void OnDamageAreaBodyEntered(object body)
	{
		if (body is Enemy enemy)
			enemy.TakeDamage(damage * 0.5f);
	}
}