using Godot;
using System;
using System.Collections.Generic;

class BigEnemy : Enemy
{
    [Export] public int maxBullets;

    protected override void Shoot(float delta)
    {
        List<EnemyBullet> bullets = new List<EnemyBullet>();

        if (timeBetweenShots <= 0)
        {
            for (int i = 0; i < maxBullets; i++)
            {
                var bullet = InstanceBullet(enemyBulletScene);
                bullets.Add(bullet);
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                float rotationDegrees = Utlities.LookAtSomething(player.Position, GlobalPosition) + (45 * i);
                var bullet = bullets[i].GetNode<BulletComponent>("BulletComponent");

                bullet = Utlities.SetNode2DParams(bullet, GlobalPosition, rotationDegrees) as BulletComponent;

                GetTree().CurrentScene.AddChild(bullets[i]);
            }

            timeBetweenShots = startTimeBetweenShots;
        }
        else timeBetweenShots -= delta;
    }
}