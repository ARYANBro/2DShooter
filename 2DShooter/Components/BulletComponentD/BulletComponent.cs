using System;
using Godot;

public class BulletComponent : RigidBody2D
{
	[Export] public float speed = 500.0f;

	public override void _Ready()
	{
		LinearVelocity = -Transform.y * speed;
	}

	virtual public void OnVisibilityNotifierScreenExited()
	{
		GetParent().QueueFree();
	}
}
