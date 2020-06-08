using Godot;
using System;

public class Bullet : RigidBody2D
{
    public override void _EnterTree()
    {
        Connect("body_entered", this, "BulletCollided");
    }

    private void BulletCollided(object body)
    {
        QueueFree();
    }

    public void Fire(Vector2 lookDir, float speed)
    {
        lookDir = lookDir.Normalized();
        AddForce(new Vector2(), lookDir * speed);
    }
}