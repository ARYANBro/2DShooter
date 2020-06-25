using System;
using Godot;

abstract public class GunComponent : Node2D
{
	[Export] 
	public float fireSpeed = 500.0f;
	[Export]
	public PackedScene bulletScene;
	[Export]
	public NodePath firepointPath;
	public Position2D firepoint;

	private Vector2 mousePos;

	public override void _Ready()
	{
		firepoint = GetNode<Position2D>(firepointPath);
	}

	public override void _Process(float delta)
	{
		mousePos = GetGlobalMousePosition();
	}
	public override void _PhysicsProcess(float delta)
	{
		// Gun to look at mouse.
		Vector2 lookDir = mousePos - GlobalPosition;
		float angle = Mathf.Atan2(lookDir.y, lookDir.x);
		RotationDegrees = Mathf.Rad2Deg(angle) + 90;

		// Shoot
		if (Input.IsActionJustPressed("Shoot"))
			Shoot(lookDir);
	}

	virtual public void Shoot(Vector2 lookDir)
	{
		Node2D bulletNode2D = (Node2D)bulletScene.Instance();
		BulletComponent bullet = bulletNode2D.GetNode<BulletComponent>("Bullet Component");

		bullet.Position = firepoint.GlobalPosition;
		bullet.Rotation = Rotation;
		GetTree().CurrentScene.AddChild(bulletNode2D);
	}
}
