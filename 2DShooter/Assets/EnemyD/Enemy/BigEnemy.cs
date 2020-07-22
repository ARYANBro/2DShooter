using Godot;
using System;
using System.Collections.Generic;

class BigEnemy : Enemy
{
    [Export] public int maxBullets = 1;

    protected override void Shoot(float delta)
    {
        List<Node> bullets = new List<Node>();

        if (timeBetweenShots <= 0)
        {
            for (int i = 0; i < maxBullets; i++)
            {
                var bulletRoot = enemyBulletScene.Instance();
                bullets.Add(bulletRoot);

                float rotationDegrees = Utlities.LookAtSomething(player.Position, GlobalPosition) + (45 * i);
                var bullet = bullets[i].GetNode<Node2D>("BulletComponent");

                bullet = Utlities.SetNode2DParams(bullet, GlobalPosition, rotationDegrees) as BulletComponent;

                GetTree().CurrentScene.AddChild(bullets[i]);
            }

            timeBetweenShots = startTimeBetweenShots;
            bullets.Clear();
        }
        else timeBetweenShots -= delta;
    }

    public override void TakeDamage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            var cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");
            cameraShake.StartShake();

            EmitSignal("SEnemyDied", 25);
            EmitSignal("SSpawnPoints", GlobalPosition, 25, new Vector2(1.5f, 1.5f));

            GetParent().QueueFree();
        }

        EmitSignal("EnemyHurt");
    }
}