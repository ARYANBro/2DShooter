using System;
using Godot;

public class Gun : Node2D
{
	[Export] 
	public float fireSpeed = 500.0f;
	[Export]
	public PackedScene bulletScene;
	public Position2D firepoint;

    private Vector2 mousePos;

	public override void _Ready()
	{
		firepoint = GetNode<Position2D>("Firepoint");
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

	private void Shoot(Vector2 lookDir)
    {
        Node2D bulletNode2D = (Node2D)bulletScene.Instance();
		PlayerBullet playerBullet = bulletNode2D.GetNode<PlayerBullet>("Bullet");
		
		playerBullet.Position = firepoint.GlobalPosition;
		playerBullet.Rotation = Rotation;
		GetTree().CurrentScene.AddChild(bulletNode2D);
	}
}