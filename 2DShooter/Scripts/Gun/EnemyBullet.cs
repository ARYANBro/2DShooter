using Godot;

public class EnemyBullet : Node2D
{
	[Export] public int damage = 10;

	private void OnBulletBodyEntered(object body)
	{
		if (body is Player player)
		{
			player = GetTree().CurrentScene.GetNode<Player>("Player");
			player.TakeDamage(damage);

			var cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");
		}

		QueueFree();
	}
}