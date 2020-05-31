using Godot;

public class Player : KinematicBody2D
{
    [Export] public float speed = 5f;
    [Export] public float acceleration = 20f;
    [Export] public float bulletVelocity = 1000f;
    [Export] public float sprintAcceleration = 30f;

    private PackedScene bulletScene;
    private RigidBody2D bullet;
    private Vector2 bulletPosition;
    private Vector2 bulletLinearVelocity;
    private float distanceBetweenBulletSpawn = 15f;

    public override void _Ready()
    {
        bulletScene = ResourceLoader.Load<PackedScene>("res://Assets/Bullet/Bullet.tscn");
    }

    public override void _PhysicsProcess(float delta)
    {
        Movement(delta);
        Actions();
    }

    private void Movement(float delta)
    {
        // Movement
        Vector2 move = new Vector2();

        if (Input.IsActionPressed("MoveForward"))
        {
            move -= new Vector2(0f, speed);

            bulletPosition = new Vector2(0f, -distanceBetweenBulletSpawn);
            bulletLinearVelocity = new Vector2(0f, -bulletVelocity);
        }
        else if (Input.IsActionPressed("MoveBackward"))
        {
            move += new Vector2(0f, speed);

            bulletPosition = new Vector2(0f, distanceBetweenBulletSpawn);
            bulletLinearVelocity = new Vector2(0f, bulletVelocity);
        }
        else if (Input.IsActionPressed("MoveLeft"))
        {
            move -= new Vector2(speed, 0f);

            bulletPosition = new Vector2(-distanceBetweenBulletSpawn, 0f);
            bulletLinearVelocity = new Vector2(-bulletVelocity, 0f);
        }
        else if (Input.IsActionPressed("MoveRight"))
        {
            move += new Vector2(speed, 0f);
            bulletPosition = new Vector2(distanceBetweenBulletSpawn, 0f);
            bulletLinearVelocity = new Vector2(bulletVelocity, 0f);
        }

        move = move.Normalized();

        if (Input.IsActionPressed("Sprint"))
        {
            move = move.LinearInterpolate(move * speed, sprintAcceleration);
        }
        else
        {
            move = move.LinearInterpolate(move * speed, acceleration);
        }

        MoveAndSlide(move);
    }

    private void Actions()
    {
        // Shooting
        if (Input.IsActionJustPressed("Shoot"))
        {
            bullet = (RigidBody2D)bulletScene.Instance();
            bullet.Position = (Position + bulletPosition);
            bullet.LinearVelocity = bulletLinearVelocity;

            GetTree().Root.AddChild(bullet);
        }
    }
}