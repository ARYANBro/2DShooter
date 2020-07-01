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

		GlobalPosition = Utlities.RandPosition(new Vector2(320f, 180f), GlobalPosition);
		sprite.Material.Set("shader_param/Color", color);
		timeBetweenShots = startTimeBetweenShots;
		GetTree().CurrentScene.Connect("EnemyDiedSignal", GetTree().CurrentScene, "OnEnemyDied");
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
	}

	// Set the color back too orignal
	private void OnHitTimerTimeout()
	{
		if ((Color)sprite.Material.Get("shader_param/Color") != color)
			sprite.Material.Set("shader_param/Color", color);
		else return;
	}

    private void Shoot(float delta)
	{
		if (timeBetweenShots <= 0)
		{
			Node2D enemyBulletNode2D = enemyBulletScene.Instance() as Node2D;
			BulletComponent bullet = enemyBulletNode2D.GetNode<BulletComponent>("BulletComponent");

			bullet.RotationDegrees = Utlities.LookAtSomething(player.Position, GlobalPosition);
			bullet.Position = GlobalPosition;

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
		inputVector = player.GlobalPosition - GlobalPosition;
		inputVector = inputVector.Normalized();

		if (GlobalPosition.DistanceTo(player.GlobalPosition) > stoppingDistance)
			velocity = velocity.MoveToward(inputVector * speed, accel * delta);
		else if (GlobalPosition.DistanceTo(player.GlobalPosition) < retreatDistance)
			velocity = velocity.MoveToward(-inputVector * speed, accel * delta);
		else
			velocity = velocity.MoveToward(Vector2.Zero, accel * delta);

		velocity = MoveAndSlide(velocity);
	}
}