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

    private Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
		playerSprintParticles = GetNode<Particles2D>("Player Sprite Paritcles");
	}

	public override void _PhysicsProcess(float delta)
	{
		Movement(delta);
	}

	// Movement
	private void Movement(float delta)
	{
		Vector2 inputVector = Vector2.Zero;
		inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
		inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
		inputVector = inputVector.Normalized();

        // Sprinting 
        if (Input.IsActionPressed("Sprint"))
		{
			if (inputVector != Vector2.Zero)
				velocity = velocity.MoveToward(inputVector * sprintSpeed, sprintAccelerations * delta);
			else
				velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

			// Set sprint particles emitting to true.
			playerSprintParticles.Emitting = true;
		}
		else
		{
			// Set sprint particles emitting to false.
			playerSprintParticles.Emitting = false;

			if (inputVector != Vector2.Zero)
				velocity = velocity.MoveToward(inputVector * speed, acceleration * delta);
			else
				velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
		}

		velocity = MoveAndSlide(velocity);
	}

	// When player is damaged can be called by any one.
	public void TakeDamage(int damage)
	{
		hp -= damage;

		if (hp <= 0)
		{
			Hide();
		}

		EmitSignal("PlayerDamaged");
	}
}
