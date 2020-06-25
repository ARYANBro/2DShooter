using System;
using Godot;

public class Enemy : KinematicBody2D
{
	[Signal]
	public delegate void EnemyDiedSignal();
	[Signal]
	public delegate void EnemyHurt();

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

	[Export]
	public Color color;

	public Player player;
	public GunComponent gun;

	private Vector2 velocity;
	private float timeBetweenShots;
	private Sprite sprite;

	public override void _Ready()
	{
		GD.Randomize();
		player = GetTree().CurrentScene.GetNode<Player>("Player");
		gun = player.GetNode<GunComponent>("Gun");
		sprite = GetNode<Sprite>("Enemy Sprite");
		sprite.Material.Set("shader_param/Color", color);

		// For shooting.
		timeBetweenShots = startTimeBetweenShots;
		Position = new Vector2((float)GD.RandRange(0, 320), (float)GD.RandRange(0, 180));
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

		EmitSignal("EnemyHurt");
	}

	private void OnEnemyHurt()
	{
		// Setting color to white when enemy is damaged.
		sprite.Material.Set("shader_param/Color", new Color(1f, 1f, 1f, 1f));
		var hitTimer = GetNode<Timer>("Hit Timer");
		hitTimer.Start();
	}


	// Set the color back too orignal
	private void OnHitTimerTimeout()
	{
		sprite.Material.Set("shader_param/Color", color);
	}

	// Shooting
	private void Shoot(float delta)
	{
		if (timeBetweenShots <= 0)
		{
			Node2D enemyBulletNode2D = (Node2D)enemyBulletScene.Instance();
			BulletComponent enemyBullet = enemyBulletNode2D.GetNode<BulletComponent>("Bullet");
			Vector2 playerPos = enemyBullet.Position - player.Position;
			enemyBullet.Position = Position;

			GetTree().CurrentScene.AddChild(enemyBulletNode2D);

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
