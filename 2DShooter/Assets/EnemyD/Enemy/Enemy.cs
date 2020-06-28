using System;
using Godot;

public class Enemy : KinematicBody2D
{
	[Signal]public delegate void EnemyDiedSignal();
	[Signal] public delegate void EnemyHurt();

	[Export] public int speed;
	[Export] public int accel;
	[Export] public int stoppingDistance;
	[Export] public float Hp
	{
		get
		{
			return hp;
		}
		private set
		{
			hp = value;
			Mathf.Clamp(hp, 0, 50);
		}
	}
	[Export] public float hitTimerWaitTime;
	[Export] public int retreatDistance;
	[Export] public PackedScene enemyBulletScene;
	[Export] public float startTimeBetweenShots;
	[Export] public Color color;
	public Player player;

	private Vector2 velocity;
	private float timeBetweenShots;
	private Sprite sprite;
	private float hp;

	public override void _Ready()
	{
		GD.Randomize();
		player = GetTree().CurrentScene.GetNode<Player>("Player");
		sprite = GetNode<Sprite>("EnemySprite");
		sprite.Material.Set("shader_param/Color", color);
		timeBetweenShots = startTimeBetweenShots;
		Position = new Vector2((float)GD.RandRange(0, 320), (float)GD.RandRange(0, 180));
	}

	public override void _PhysicsProcess(float delta)
	{
		Movement(delta);
		Shoot(delta);
	}

	public void TakeDamage(float damage)
	{
		Hp -= damage;
		if (Hp <= 0)
		{
			CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");
			cameraShake.StartShake();
			EmitSignal("EnemyDiedSignal");
			QueueFree();
		}

		EmitSignal("EnemyHurt");
	}

	private void OnEnemyHurt()
	{
		sprite.Material.Set("shader_param/Color", new Color(1f, 1f, 1f, 1f));
		GetTree().CreateTimer(hitTimerWaitTime).Connect("timeout", this, "OnHitTimerTimeout");
		Action OnHitTimerTimeout = () => sprite.Material.Set("shader_param/Color", color);
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
			BulletComponent bullet = enemyBulletNode2D.GetNode<BulletComponent>("BulletComponent");

			Vector2 playerPos = Position - player.Position;
			float angle = Mathf.Atan2(playerPos.y, playerPos.x);

			bullet.Rotation = angle - 1.5708f; // Adding 90 degrees in radians
			bullet.Position = Position;
			GetTree().CurrentScene.AddChild(enemyBulletNode2D);
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