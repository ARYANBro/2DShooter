using Godot;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;

public class Enemy : KinematicBody2D
{
	[Export] public float acceleration = 1f;
	public KinematicBody2D player;
	public Area2D enemyDetection;

	private bool caughtPlayer = false;

	public override void _Ready()
	{
		SetPhysicsProcess(true);

		player = (KinematicBody2D)GetTree().Root.FindNode("Player", true, false);

		enemyDetection = GetNode<Area2D>("EnemyDetection");
		enemyDetection.Connect("body_entered", this, "CanSeePlayer");
		enemyDetection.Connect("body_exited", this, "CannotSeePlayer");
	}

	public override void _PhysicsProcess(float delta)
	{
		if (Position != player.Position && !caughtPlayer)
		{
			Position = Position.LinearInterpolate(player.Position, acceleration * delta);
		}
	}

	private void CanSeePlayer(object body)
	{
		if (body.GetType().Name == "Player")
		{
			caughtPlayer = true;
		}
	}

	private void CannotSeePlayer(object body)
	{
		if (body.GetType().Name == "Player")
		{
			caughtPlayer = false;
		}
	}
}
