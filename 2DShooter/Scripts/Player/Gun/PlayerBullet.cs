using Godot;
using System;

public class PlayerBullet : Bullet
{
	[Export] 
	public int damage = 20;
	[Export]
	public PackedScene hitParticlesScene;

	private void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Enemy")
		{
			Enemy enemy = (Enemy)body;
			enemy.TakeDamage(damage);
		}

		var hitParticle = (Particles2D)hitParticlesScene.Instance();
		hitParticle.Position = Position;
		hitParticle.Emitting = true;

        // Delete the hit particle #TODO
        /******************************/

        GetTree().CurrentScene.AddChild(hitParticle);
        QueueFree();
    }
}
