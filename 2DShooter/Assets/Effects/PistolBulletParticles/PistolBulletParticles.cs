using Godot;
using System;

public class PistolBulletParticles : Particles2D
{
	public override void _Ready()
	{
		GetTree().CreateTimer(1f).Connect("timeout", this, "queue_free");
	}

	void DeleteParticle() => QueueFree();
}