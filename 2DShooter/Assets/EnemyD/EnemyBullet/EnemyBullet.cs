using System;
using Godot;

public class EnemyBullet : Node2D
{
	[Export]
	public int damage = 10;

	public override void _Ready()
	{
		var playerPos = -Position.DirectionTo(GetTree().CurrentScene.GetNode<Player>("Player").Position);
		float angle = Mathf.Atan2(playerPos.y, playerPos.x);
		RotationDegrees = Mathf.Rad2Deg(angle) - 90;
		//LinearVelocity = -Transform.y * speed;
	}

	protected void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			Player player = GetTree().CurrentScene.GetNode<Player>("Player");
			player.TakeDamage(damage);
			
			// Camera shake!! yay!
			CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("Main Cam");
			cameraShake.Shake(50.0f, 50.0f, 50.0f);
		}

		QueueFree();
	}
}
