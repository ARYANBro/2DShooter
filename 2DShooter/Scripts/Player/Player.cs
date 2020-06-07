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
        Vector2 direction = new Vector2();

        if (Input.IsActionPressed("MoveUp"))
            direction.y -= Position.y;
        else if (Input.IsActionPressed("MoveDown"))
            direction.y += Position.y;

        if (Input.IsActionPressed("MoveLeft"))
            direction.x -= Position.x;

        else if (Input.IsActionPressed("MoveRight"))
            direction.x += Position.x;

        direction = direction.Normalized();
        
        if (Input.IsActionPressed("Sprint"))
            velocity = velocity.LinearInterpolate(direction * sprintSpeed, accel * delta);
        else
            velocity = velocity.LinearInterpolate(direction * speed, accel * delta);

        MoveAndSlide(velocity);
    }
}
