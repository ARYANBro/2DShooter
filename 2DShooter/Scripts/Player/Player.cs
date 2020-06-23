using System;
using Godot;

public class Player : KinematicBody2D
{
	[Signal]
	public delegate void PlayerDamaged();

	[Export]
	public float sprintSpeed;
	[Export]
	public float acceleration;
	[Export]
	public float sprintAccelerations;
	[Export]
	public float friction;
	[Export]
	public float speed;
	[Export]
	public int hp;
	public Particles2D playerSprintParticles;
	public bool canSprint = true;
	public float stamina = 400f;

	public Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
		playerSprintParticles = GetNode<Particles2D>("Player Sprite Paritcles");
	}

	public override void _Process(float delta)
	{
		// hp and stamina.
		hp = Mathf.Clamp(hp, 0, 100);
		stamina = Mathf.Clamp((int)stamina, 0, 400);

		if (Input.IsActionPressed("Sprint") && canSprint && velocity != Vector2.Zero)
			stamina -= 3;
		else
			stamina += 1;

		if (stamina <= 0)
			canSprint = false;
		else if (stamina >= 400)
			canSprint = true;

		// Movement.
		Vector2 inputVector = Vector2.Zero;
		inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
		inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
		inputVector = inputVector.Normalized();

		// Sprinting 
		if (Input.IsActionPressed("Sprint") && canSprint)
		{
			if (inputVector != Vector2.Zero)
				velocity = velocity.MoveToward(inputVector * sprintSpeed, sprintAccelerations * delta);
			else
				velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

			playerSprintParticles.Emitting = true;
		}
		else
		{
			playerSprintParticles.Emitting = false;

			if (inputVector != Vector2.Zero)
				velocity = velocity.MoveToward(inputVector * speed, acceleration * delta);
			else
				velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		velocity = MoveAndSlide(velocity);
	}


	// When player is damaged can be called by any one.
	public void TakeDamage(int damage)
	{
		hp -= damage;
		EmitSignal("PlayerDamaged");
	}
	
	private void OnPlayerDied()
	{
		Hide();
	}
}
