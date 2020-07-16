using System;
using Godot;

public class GunComponent : Node2D
{
    [Export] public float fireSpeed;
    [Export] public float startTimeBetweenShots;
    [Export] public PackedScene bulletScene;
    [Export] public NodePath firepointPath;
    public Position2D firepoint;

	private Vector2 lookDir;

    protected float timeBetweenShots;

    public override void _Ready()
    {
        firepoint = GetNode<Position2D>(firepointPath);
    }

    public override void _Process(float delta)
    {
        lookDir = GetGlobalMousePosition() - GlobalPosition;
        RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), GlobalPosition);
        Shoot(lookDir, delta);
    }

    virtual public void Shoot(Vector2 lookDir, float delta)
    {
        if (timeBetweenShots <= 0)
        {
            if (Input.IsActionPressed("Shoot"))
            {
                Node bulletNode2D = bulletScene.Instance();
                var bullet = bulletNode2D.GetNode<BulletComponent>("BulletComponent");

                bullet.Position = firepoint.GlobalPosition;
                bullet.Rotation = Rotation;
                bullet.speed = fireSpeed;

                GetTree().CurrentScene.AddChild(bulletNode2D);

                timeBetweenShots = startTimeBetweenShots;
            }
        }
        else timeBetweenShots -= delta;
    }
    public BulletComponent InstanceBullet(PackedScene scene)
    {
        var bulletRoot = scene.Instance() as BulletComponent;
        return bulletRoot;
    }
}