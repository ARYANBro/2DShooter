using System;
using Godot;

public class GunComponent : Node2D
{
    [Export] public float fireSpeed;
    [Export] public float startTimeBetweenShots;
    [Export] public PackedScene bulletScene;
    [Export] public NodePath firepointPath;
    [Export] public NodePath gunShooAudioPath;
    public Position2D firepoint;

	private Vector2 lookDir;
    private AudioStreamPlayer2D gunShootAudio;

    protected float timeBetweenShots;

    public override void _Ready()
    {
        firepoint = GetNode<Position2D>(firepointPath);
        gunShootAudio = GetNode<AudioStreamPlayer2D>(gunShooAudioPath);
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

                // Play Audio

                gunShootAudio.Play();
            }
        }
        else
            timeBetweenShots -= delta;
    }
}