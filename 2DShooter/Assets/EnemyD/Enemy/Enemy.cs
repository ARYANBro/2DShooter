using System;
using Godot;

public class Enemy : KinematicBody2D
{
    [Signal] public delegate void SEnemyDied(float points);
	[Signal] public delegate void SSpawnPoints(Vector2 position);
    [Signal] public delegate void EnemyHurt();

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
    [Export] public Color color;
    public Player player;

    protected float timeBetweenShots;
    protected Vector2 velocity;
    protected Sprite sprite;
    protected float hp;

    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        sprite = GetNode<Sprite>("EnemySprite");

        GlobalPosition = Utlities.RandPosition(new Vector2(320f, 180f), GlobalPosition);

        sprite.Material.SetDeferred("shader_param/Color", color);
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

    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            CameraShake cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");

            cameraShake.StartShake();
            EmitSignal("SEnemyDied", 15);
			EmitSignal("SSpawnPoints", GlobalPosition);
            GetParent().QueueFree();
        }

        EmitSignal("EnemyHurt");
    }

    private void OnEnemyHurt()
    {
        sprite.Material.Set("shader_param/Color", new Color(1f, 1f, 1f, 1f));
        GetTree().CreateTimer(hitTimerWaitTime).Connect("timeout", this, "OnHitTimerTimeout");
    }

    private void OnHitTimerTimeout()
    {
        if ((Color)sprite.Material.Get("shader_param/Color") != color)
            sprite.Material.SetDeferred("shader_param/Color", color);
        else return;
    }

    virtual protected void Shoot(float delta)
    {
        if (timeBetweenShots <= 0)
        {
            var bulletRoot = InstanceBullet(enemyBulletScene);
            var bullet = bulletRoot.GetNode<BulletComponent>("BulletComponent");

            float rotationDegrees = Utlities.LookAtSomething(player.Position, GlobalPosition);
            bullet = Utlities.SetNode2DParams(bullet, GlobalPosition, rotationDegrees) as BulletComponent;
            GetTree().CurrentScene.AddChild(bulletRoot);

            timeBetweenShots = startTimeBetweenShots;
        }
        else
            timeBetweenShots -= delta;
    }

    private void Move(float delta)
    {
        velocity = MoveAndSlide(velocity);
    }

    private void GetMovementInput(float delta)
    {
        Vector2 inputVector = Vector2.Zero;
        inputVector = player.GlobalPosition - GlobalPosition;
        inputVector = inputVector.Normalized();

        if (GlobalPosition.DistanceTo(player.GlobalPosition) > stoppingDistance)
            velocity = velocity.MoveToward(inputVector * speed, accel * delta);
        else if (GlobalPosition.DistanceTo(player.GlobalPosition) < retreatDistance)
            velocity = velocity.MoveToward(-inputVector * speed, accel * delta);
        else
            velocity = velocity.MoveToward(Vector2.Zero, accel * delta);
    }

    protected EnemyBullet InstanceBullet(PackedScene scene)
    {
        var bulletRoot = scene.Instance() as EnemyBullet;
        return bulletRoot;
    }
}