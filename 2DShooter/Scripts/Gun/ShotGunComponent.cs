using Godot;
using System.Collections.Generic;

public class ShotGunComponent : GunComponent
{
    [Export] public int maxBullets = 1;
    public Player player;
    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        firepoint = GetNode<Position2D>(firepointPath);
    }

    public override void Shoot(Vector2 lookDir, float delta)
    {
        List<ShotgunShell> bullets = new List<ShotgunShell>();

        if (timeBetweenShots <= 0)
        {

            if (Input.IsActionPressed("Shoot"))
            {
                for (int i = 0; i < maxBullets; i++)
                {
                    var bulletRoot = bulletScene.Instance() as ShotgunShell;
                    bullets.Add(bulletRoot);
                }

                for (int i = 0; i < bullets.Count; i++)
                {
                    float rotationDegrees = Utlities.LookAtSomething(GetGlobalMousePosition(), GlobalPosition) + (20 * i);
                    var bullet = bullets[i].GetNode<BulletComponent>("BulletComponent");

                    Utlities.SetNode2DParams(ref bullet, firepoint.GlobalPosition, rotationDegrees);

                    GetTree().CurrentScene.AddChild(bullets[i]);
                }

                timeBetweenShots = startTimeBetweenShots;
                bullets.Clear();
            }

        }
        else timeBetweenShots -= delta;
    }
}
