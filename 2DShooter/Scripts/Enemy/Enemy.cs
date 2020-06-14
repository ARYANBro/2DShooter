using System;
using Godot;

public class Enemy : KinematicBody2D
{
    [Signal] public delegate void EnemyDiedSignal();

    [Export] public float acceleration = 450.0f;
    [Export] public float friction = 100.0f;
    [Export] public float maxSpeed = 50.0f;
    [Export] public int hp = 50;

    public PackedScene bulletScene;
    public Timer attackRateTimer;
    public Area2D detection;
    public Player player;
    public Gun gun;

    private bool canSeePlayer;
    private Vector2 velocity;
    private float angle;

    public override void _Ready()
    {
        bulletScene = ResourceLoader.Load<PackedScene>("res://Assets/Enemy/Enemy Bullet.tscn");
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        attackRateTimer = GetNode<Timer>("Attack Rate");
        detection = GetNode<Area2D>("Detection Area");
        gun = player.GetNode<Gun>("Gun");
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
        else
            attackRateTimer.SetBlockSignals(true);

    }

    private void Movement(float delta)
    {
        if (player != null)
        {
            Vector2 inputVector = Vector2.Zero;
            inputVector = player.GlobalPosition - GlobalPosition;
            inputVector = inputVector.Normalized();

            if (canSeePlayer)
                velocity = velocity.MoveToward(inputVector * maxSpeed, acceleration * delta);
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

            velocity = MoveAndSlide(velocity);
        }
    }

    private void Attack()
    {
        Vector2 offset = GlobalPosition - player.Position;
        offset = offset.Normalized();

        var bullet = (Node2D)bulletScene.Instance(PackedScene.GenEditState.Instance);
        var enemyBullet = bullet.GetNode<EnemyBullet>("Bullet");

        enemyBullet.Position = Position;
        enemyBullet.GlobalRotationDegrees = angle;
        enemyBullet.GlobalPosition += offset * -15.0f;

        GetTree().CurrentScene.AddChild(bullet);
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
        {
            var cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("Main Cam");
            cameraShake.StartShake();
            EmitSignal("EnemyDiedSignal");
            QueueFree();
        }
    }

    private void OnAttackRateTimerTimeOut()
    {
        Attack();
    }
}