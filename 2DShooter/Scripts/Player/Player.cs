using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public float sprintSpeed;
    [Export] public float speed;
    [Export] public float accel;

    private Vector2 velocity;

    public override void _PhysicsProcess(float delta)
    {
        Movement(delta);
    }

    private void Movement(float delta)
    {
        Vector2 inputVector = new Vector2();

        inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");

        GD.Print(inputVector);

        if (Input.IsActionPressed("Sprint"))
            velocity = velocity.LinearInterpolate(inputVector * sprintSpeed, accel * delta);
        else
            velocity = velocity.LinearInterpolate(inputVector * speed, accel * delta);

        MoveAndSlide(velocity);
    }
}
