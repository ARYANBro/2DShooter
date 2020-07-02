using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Godot;

public class Enemy : KinematicBody2D
{
	[Signal] public delegate void EnemyDiedSignal();
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

	private float timeBetweenShots;
	private Vector2 velocity;
	private Sprite sprite;
	private float hp;
	private bool retreat;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
		sprite = GetNode<Sprite>("EnemySprite");

		GlobalPosition = Utlities.RandPosition(new Vector2(320f, 180f), GlobalPosition);

		sprite.Material.SetDeferred("shader_param/Color", color);
		timeBetweenShots = startTimeBetweenShots;
	}

	public override void _Process(float delta)
	{
		GetMovementInput(delta);
		Shoot(delta);
	}

	public override void _PhysicsProcess(float delta)
	{
		Move(delta);
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

	private void OnHitTimerTimeout()
	{
		if ((Color)sprite.Material.Get("shader_param/Color") != color)
			sprite.Material.SetDeferred("shader_param/Color", color);
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

	private void Move(float delta)
	{
		velocity = MoveAndSlide(velocity);
	}

	private void GetMovementInput(float delta)
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
	}
}
