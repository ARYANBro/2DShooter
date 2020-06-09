using Godot;
using System;

public class Bullet : RigidBody2D {
    public override void _EnterTree() {
        Connect("body_entered", this, "BulletCollided");
    }

    private void BulletCollided(object body) {
        QueueFree();
    }
}