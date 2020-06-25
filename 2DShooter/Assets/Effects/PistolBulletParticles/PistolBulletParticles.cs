using Godot;
using System;
using System.Collections;

public class PistolBulletParticles : Particles2D
{
	public Tween tween;

	public override void _Ready()
	{
		tween = GetNode<Tween>("Tween");
		tween.InterpolateCallback(this, 1.0f, "DeleteParticle");
		tween.Start();
	}

	void DeleteParticle()
	{
		QueueFree();
	}

}
