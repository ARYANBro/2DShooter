using System;
using Godot;
public class EnemyBullet : Node2D
{
	[Export] public int damage = 10;

	void OnBulletBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			Player player = GetTree().CurrentScene.GetNode<Player>("Player");
			player.TakeDamage(damage);

			CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");
			cameraShake.Shake(50.0f, 50.0f, 50.0f);
		}
		QueueFree();
	}
}