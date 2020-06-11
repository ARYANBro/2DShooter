using System;
using Godot;

public class Enemy : KinematicBody2D
{
    [Export] public float acceleration = 450.0f;
    [Export] public float friction = 100.0f;
    [Export] public float maxSpeed = 50.0f;
    [Export] public int hp = 50;

    public PackedScene bulletScene;
    public Area2D detection;
    public Player player;
    public Gun gun;

    private Vector2 velocity;
    private bool canSeePlayer;

    public override void _Ready()
    {
        bulletScene = (PackedScene)ResourceLoader.Load("res://Assets/Player/Gun/Bullet.tscn");
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        detection = GetNode<Area2D>("Detection Area");
        gun = player.GetNode<Gun>("Gun");
    }

    public override void _Process(float delta)
    {
        Vector2 lookDir = player.GlobalPosition - GlobalPosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);
        angle = Mathf.Rad2Deg(angle) + 90;

        if (canSeePlayer)
        {
            Attack(angle);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Movement(delta);
    }

    private void Movement(float delta)
    {
        if (player != null)
        {
            Vector2 inputVector = Vector2.Zero;
            inputVector = player.GlobalPosition - GlobalPosition;
            inputVector = inputVector.Normalized();

            if (canSeePlayer)
            {
                velocity = velocity.MoveToward(inputVector * maxSpeed, acceleration * delta);
            }
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

            velocity = MoveAndSlide(velocity);
        }
    }

    private void Attack(float angle)
    {
        if (bulletScene != null)
        {
            RigidBody2D bullet = (RigidBody2D)bulletScene.Instance(PackedScene.GenEditState.Instance);
            CollisionShape2D bulletCollision = bullet.GetNode<CollisionShape2D>("Bullet Collision");
            bullet.Position = Position;
            bullet.RotationDegrees = angle;
            bulletCollision.Disabled = true;
 
            GetTree().Root.AddChild(bullet);
        }
    }

    public void OnDetectionAreaEntered(object body)
    {
        if (body.GetType().Name == "Player")
            canSeePlayer = true;
    }

    public void OnDetectoinAreaBodyExited(object body)
    {
        if (body.GetType().Name == "Player")
            canSeePlayer = false;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
            QueueFree();
    }
}
