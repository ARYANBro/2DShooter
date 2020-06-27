using System;
using Godot;

abstract public class GunComponent : Node2D
{
	[Export] public float fireSpeed;
	[Export] public float startTimeBetweenShots;
	[Export] public PackedScene bulletScene;
	[Export] public NodePath firepointPath;
	public Position2D firepoint;

	private float timeBetweenShots;
	private Vector2 mousePos;

	public override void _Ready() => firepoint = GetNode<Position2D>(firepointPath);

	public override void _Process(float delta)
	{
		mousePos = GetGlobalMousePosition();

		// Gun to look at mouse.
		Vector2 lookDir = mousePos - GlobalPosition;
		float angle = Mathf.Atan2(lookDir.y, lookDir.x);
		RotationDegrees = Mathf.Rad2Deg(angle) + 90;
		
		Shoot(lookDir, delta);
	}

	virtual public void Shoot(Vector2 lookDir, float delta)
	{
		if (timeBetweenShots <= 0)
		{
			if (Input.IsActionPressed("Shoot"))
			{
				Node2D bulletNode2D = (Node2D)bulletScene.Instance();
				BulletComponent bullet = bulletNode2D.GetNode<BulletComponent>("BulletComponent");

				bullet.Position = firepoint.GlobalPosition;
				bullet.Rotation = Rotation;
				bullet.speed = fireSpeed;
				GetTree().CurrentScene.AddChild(bulletNode2D);
				timeBetweenShots = startTimeBetweenShots;
			}
		}
		else
			timeBetweenShots -= delta;
	}
}
