using System;
using Godot;
public class Enemy : KinematicBody2D
{
	[Signal] 
	public delegate void EnemyDiedSignal();

	[Export]
	public int speed;
	[Export]
	public int accel;
	[Export]
	public int stoppingDistance;
	[Export]
	public int retreatDistance;
	[Export]
	public int hp;
	[Export]
	public PackedScene enemyBulletScene;
	[Export]
	public float startTimeBetweenShots;

	public Player player;
	public Gun gun;

	private Vector2 velocity;
	private float timeBetweenShots;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
		gun = player.GetNode<Gun>("Gun");

		timeBetweenShots = startTimeBetweenShots;
	}


	public override void _PhysicsProcess(float delta)
	{
		Movement(delta);
		Shoot(delta);
	}

	// Can be called whenever enemy has to be damaged.
	public void TakeDamage(int damage)
	{
		hp -= damage;

		if (hp <= 0)
		{
			CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("Main Cam");
			cameraShake.StartShake();
			EmitSignal("EnemyDiedSignal");
			QueueFree();
		}
	}

	// Shooting
	private void Shoot(float delta)
	{	
		if (timeBetweenShots <= 0)
		{
			Node2D enemyBullet = (Node2D)enemyBulletScene.Instance();
			EnemyBullet enemyBulletRigid = enemyBullet.GetNode<EnemyBullet>("Bullet");

			Vector2 playerPos = enemyBulletRigid.GlobalPosition - player.GlobalPosition;
			enemyBulletRigid.Position = Position;

			GetTree().CurrentScene.AddChild(enemyBullet);

			// Setting this to "startTimeBetweenShots" is important.
			timeBetweenShots = startTimeBetweenShots;
		}
		else
			timeBetweenShots -= delta;

	}

	// Movement
	private void Movement(float delta)
	{
		Vector2 inputVector = Vector2.Zero;
		inputVector = player.Position - Position;
		inputVector = inputVector.Normalized();

		if (Position.DistanceTo(player.Position) > stoppingDistance)
			velocity = velocity.MoveToward(inputVector * speed, accel * delta);
		else if (Position.DistanceTo(player.Position) < retreatDistance)
			velocity = velocity.MoveToward(-inputVector * speed, accel * delta);
		else
			velocity = velocity.MoveToward(Vector2.Zero, accel * delta);

		MoveAndSlide(velocity);

	}
}
