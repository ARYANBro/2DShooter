using System;
using Godot;

public class Enemy : KinematicBody2D
{
    [Signal] public delegate void SEnemyDied(float points);
    [Signal] public delegate void EnemyHurt();
    [Signal] public delegate void SSpawnPoints(Vector2 position, int points, Vector2 size);

    [Export] public int speed;
    [Export] public int accel;
    [Export] public int stoppingDistance;
    [Export]
    public float Hp
    {
        get
        {
            return hp;
        }
        protected set
        {
            hp = value;
            Mathf.Clamp(hp, 0, 50);
        }
    }
    [Export] public float hitTimerWaitTime;
    [Export] public int retreatDistance;
    [Export] public PackedScene enemyBulletScene;
    [Export] public float startTimeBetweenShots;
    public Player player;

    protected float timeBetweenShots;
    protected Vector2 velocity;
    protected Sprite sprite;
    protected float hp;

    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        sprite = GetNode<Sprite>("EnemySprite");

        GlobalPosition = Utlities.RandPosition(new Vector2(320f, 180f), GetTree());
        timeBetweenShots = startTimeBetweenShots;

        Connect("SEnemyDied", GetTree().CurrentScene, "OnEnemyDied");
        Connect("SSpawnPoints", GetTree().CurrentScene, "SpawnPoints");
    }

    public override void _Process(float delta)
    {
        GetMovementInput(delta);
        Shoot(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        Move(delta);
    }

    async public virtual void TakeDamage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            var cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");

            cameraShake.StartShake();

            EmitSignal("SEnemyDied", 15);
            EmitSignal("SSpawnPoints", GlobalPosition, 15, new Vector2(1f, 1f));

            GetParent().QueueFree();
        }

        GetNode<Sprite>("EnemySprite").Material.Set("shader_param/hit", true);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetNode<Sprite>("EnemySprite").Material.Set("shader_param/hit", false);
    }
    public virtual void Shoot(float delta)
    {
        if (timeBetweenShots <= 0)
        {
            if (enemyBulletScene != null)
            {
                var bulletRoot = enemyBulletScene.Instance();
                var bullet = bulletRoot.GetNode<BulletComponent>("BulletComponent");

                float rotationDegrees = Utlities.LookAtSomething(player.Position, GlobalPosition);
                Utlities.SetNode2DParams(ref bullet, GlobalPosition, rotationDegrees);

                GetTree().CurrentScene.AddChild(bulletRoot);
                timeBetweenShots = startTimeBetweenShots;
            }
            else return;
        }
        else timeBetweenShots -= delta;
    }

    void Move(float delta)
    {
        velocity = MoveAndSlide(velocity);
    }

    protected void GetMovementInput(float delta)
    {
        var inputVector = Vector2.Zero;
        inputVector = player.GlobalPosition - GlobalPosition;
        inputVector = inputVector.Normalized();
        if (GlobalPosition.DistanceTo(player.GlobalPosition) > stoppingDistance)
            velocity = velocity.MoveToward(inputVector * speed, accel * delta);
        else if (GlobalPosition.DistanceTo(player.GlobalPosition) < retreatDistance)
            velocity = velocity.MoveToward(-inputVector * speed, accel * delta);
        else velocity = velocity.MoveToward(Vector2.Zero, accel * delta);
    }
}