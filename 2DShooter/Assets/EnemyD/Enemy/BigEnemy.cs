using Godot;
using System;
using System.Collections.Generic;

class BigEnemy : Enemy
{
    [Export] public int maxBullets = 1;

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

    public override void TakeDamage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");

            cameraShake.StartShake();
            EmitSignal("SEnemyDied", 25);
            EmitSignal("SSpawnPoints", GlobalPosition, 25, new Vector2(1.5f, 1.5f));

            GetParent().QueueFree();
        }

        EmitSignal("EnemyHurt");
    }
}