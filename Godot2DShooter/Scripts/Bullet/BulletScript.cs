using Godot;

public class BulletScript : RigidBody2D
{
	public Timer bulletLife;
	public Particles2D collidingParticles;
	public AnimationPlayer bulletDyedAnimation;
	public Timer bulletAnimationTimer;

	public override void _Ready()
	{
		bulletLife = GetNode<Timer>("BulletLife");
		collidingParticles = GetNode<Particles2D>("CollidingParticles");
		bulletDyedAnimation = GetNode<AnimationPlayer>("BulletDyed");
		bulletAnimationTimer = GetNode<Timer>("BulletDyed/BulletAnimationTimer");
	}

	private void BodyEntered(object body)
	{
		collidingParticles.Emitting = true;
		Sleeping = true;

		bulletLife.Start();
	}

	private void BulletLifeTimeout()
	{
		bulletDyedAnimation.Play("BulletDyed");
		bulletAnimationTimer.Start();
	}

	private void BulletDyedAnimationTimer()
	{
		QueueFree();
	}
}
