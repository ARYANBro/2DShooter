using System;
using Godot;

public class Enemy : KinematicBody2D
{
    [Export] public float acceleration = 450.0f;
    [Export] public float friction = 100.0f;
    [Export] public float maxSpeed = 50.0f;
    [Export] public int hp = 50;

    public PackedScene bulletScene;
    public Timer attackRateTimer;
    public Area2D detection;
    public Player player;
    public Gun gun;

    private Vector2 velocity;
    private bool canSeePlayer;
    float angle;

    public override void _Ready()
    {
        bulletScene = (PackedScene)ResourceLoader.Load("res://Assets/Player/Gun/Bullet.tscn");
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        attackRateTimer = GetNode<Timer>("Attack Rate");
        detection = GetNode<Area2D>("Detection Area");
        gun = player.GetNode<Gun>("Gun");
        attackRateTimer.SetBlockSignals(true);
    }

    public override void _Process(float delta)
    {
        Vector2 lookDir = player.GlobalPosition - GlobalPosition;
        angle = Mathf.Atan2(lookDir.y, lookDir.x);
        angle = Mathf.Rad2Deg(angle) + 90;
    }

    public override void _PhysicsProcess(float delta)
    {
        Movement(delta);
        if (canSeePlayer)
            attackRateTimer.SetBlockSignals(false);
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

    private void Attack()
    {
        RigidBody2D bullet = (RigidBody2D)bulletScene.Instance();
        bullet.GetNode<CollisionShape2D>("Bullet Collision").Disabled = true;
        bullet.CollisionLayer = Convert.ToUInt32(2);
        bullet.CollisionMask = Convert.ToUInt32(1);
        bullet.CollisionMask = Convert.ToUInt32(3);
        bullet.RotationDegrees = angle;
        bullet.Position = Position;
        GetTree().Root.AddChild(bullet);
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

    private void OnAttackRateTimerTimeOut()
    {
        Attack();
    }
}
