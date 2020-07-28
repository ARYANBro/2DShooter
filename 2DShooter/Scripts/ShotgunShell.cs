using Godot;
using System;

public class ShotgunShell : Node2D
{
    [Export] public int damage = 50;

    public void OnBulletBodyEntered(object body)
    {
        var cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");
        if (body.GetType().Name == "Enemy")
        {
            cameraShake.StartShake();
            var enemy = body as Enemy;
            enemy.TakeDamage(damage);
        }
        else if (body.GetType().Name == "BigEnemy")
        {
            cameraShake.StartShake();
            var enemy = body as BigEnemy;
            enemy.TakeDamage(damage);
        }
		
        QueueFree();
    }
}
