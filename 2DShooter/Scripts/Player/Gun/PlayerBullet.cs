using Godot;
using System;

public class PlayerBullet : Bullet
{
    [Export] public int damage = 20;

    private void OnBulletBodyEntered(object body)
    {
        if (body.GetType().Name == "Enemy")
        {
            Enemy enemy = (Enemy)body;
            enemy.TakeDamage(damage);
        }

        QueueFree();
    }

}
