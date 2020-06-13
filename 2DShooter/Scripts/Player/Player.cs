using System;
using Godot;

public class Player : KinematicBody2D
{
    [Signal] public delegate void PlayerDiedSignal();
    [Signal] public delegate void PlayerDamaged();

    [Export] public float maxSprintSpeed = 120.0f;
    [Export] public float acceleration = 500.0f;
    [Export] public float friction = 500.0f;
    [Export] public float maxSpeed = 80.0f;
    [Export] public int hp = 100;

    private Vector2 velocity = Vector2.Zero;

    public override void _PhysicsProcess(float delta)
    {
        Movement(delta);
    }

    private void Movement(float delta)
    {
        Vector2 inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
        inputVector = inputVector.Normalized();

        if (Input.IsActionPressed("Sprint"))
        {
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * maxSprintSpeed, acceleration * delta);
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
        }
        else
        {
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * maxSpeed, acceleration * delta);
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
        }

        velocity = MoveAndSlide(velocity);
    }

    public void takeDamage(int damage)
    {
        hp -= damage;
        GD.Print(hp);

        if (hp <= 0)
        {
            Hide();
            PauseMode = PauseModeEnum.Stop;
            EmitSignal("PlayerDiedSignal");
        }

        EmitSignal("PlayerDamaged");
    }
}