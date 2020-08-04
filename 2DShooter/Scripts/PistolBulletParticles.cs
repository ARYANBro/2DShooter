using Godot;
using System;

public class PistolBulletParticles : Particles2D
{
	async public override void _Ready()
	{
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		QueueFree();
	}

	void DeleteParticle() => QueueFree();
}