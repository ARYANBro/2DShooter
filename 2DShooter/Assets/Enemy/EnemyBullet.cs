using System;
using Godot;

public class EnemyBullet : Bullet
{
    private void OnBulletBodyEntered(object body)
    {
        if (body.GetType().Name == "Player")
        {
            Player player = GetTree().CurrentScene.GetNode<Player>("Player");

            if (player != null)
            {
                player.takeDamage(damage);
            }

            CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("Main Cam");
            cameraShake.Shake(50.0f, 50.0f, 50.0f);
            QueueFree();
        }
    }
}