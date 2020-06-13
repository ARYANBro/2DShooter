using System;
using Godot;

public class EnemyBullet : Bullet
{
    [Export] public int damage = 10;

    private void OnBulletBodyEntered(object body)
    {
        if (body.GetType().Name == "Player")
        {
            var player = GetTree().CurrentScene.GetNode<Player>("Player");
            player.takeDamage(damage);

            CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("Main Cam");
            cameraShake.Shake(50.0f, 50.0f, 50.0f);
        }

        QueueFree();
    }
}