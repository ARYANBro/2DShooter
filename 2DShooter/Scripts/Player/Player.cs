using Godot;
using System;

public class Player : KinematicBody2D {
    [Export] public float maxSprintSpeed;
    [Export] public float acceleration;
    [Export] public float friction;
    [Export] public float maxSpeed;

    private Vector2 velocity = Vector2.Zero;

    public override void _PhysicsProcess(float delta) {
        Movement(delta);
    }

    private void Movement(float delta) {
        Vector2 inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
        inputVector = inputVector.Normalized();

        if (Input.IsActionPressed("Sprint")) {
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * maxSprintSpeed, acceleration * delta);
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
        } else {
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * maxSpeed, acceleration * delta);
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
        }

        velocity = MoveAndSlide(velocity);
    }
}