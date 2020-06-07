using Godot;

public class Player : KinematicBody2D
{
    /* Paths */
    [Export] public NodePath firePointSpritePath;
    [Export] public PackedScene bulletScene;
    [Export] public NodePath firePointPath;

    /* Movemnet */
    [Export] public float acceleration = 70.0f;
    
    [Export] public float sprintSpeed = 650.0f;
    [Export] public float speed = 450.0f;

    /* Ref */
    public Sprite firePointSprite;
    public Position2D firePoint; 

    /* Movement */
    private Vector2 direction;
    private Vector2 velocity;
    private Vector2 mousePos;
    private Vector2 lookDir;

    public override void _EnterTree()
    {
        firePointSprite = GetNode<Sprite>(firePointSpritePath);
        firePoint = GetNode<Position2D>(firePointPath);
    }

    public override void _Process(float delta)
    {
        mousePos = GetViewport().GetMousePosition();
    }

    public override void _PhysicsProcess(float delta)
    {
        Movement(delta);

        if (Input.IsActionJustPressed("Fire"))
            Shoot();
    }

    private void Movement(float delta)
    {
        // Movement
        direction = new Vector2();

        if (Input.IsActionPressed("MoveForward"))
            direction -= new Vector2(0.0f, 1.0f) * speed;
        else if (Input.IsActionPressed("MoveBackward"))
            direction += new Vector2(0.0f, 1.0f) * speed;

        if (Input.IsActionPressed("MoveLeft"))
            direction -= new Vector2(1.0f, 0.0f) * speed;
        else if (Input.IsActionPressed("MoveRight"))
            direction += new Vector2(1.0f, 0.0f) * speed;

        direction = direction.Normalized();

        if (Input.IsActionPressed("Sprint"))
            velocity = velocity.LinearInterpolate(direction * sprintSpeed, acceleration * delta);
        else
            velocity = velocity.LinearInterpolate(direction * speed, acceleration * delta);

        MoveAndSlide(velocity);

        //Rotate towards mouse
        lookDir = mousePos - Position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);

        firePointSprite.GlobalRotationDegrees = Mathf.Rad2Deg(angle) + 90;

    }

    private void Shoot()
    {
        Bullet bullet = (Bullet)bulletScene.Instance();

        if (bullet != null)
        {
            bullet.RotationDegrees = firePointSprite.GlobalRotationDegrees;
            bullet.Position = firePoint.GlobalPosition;
            bullet.Fire(lookDir);

            GetTree().Root.AddChild(bullet);
        }
    }
}