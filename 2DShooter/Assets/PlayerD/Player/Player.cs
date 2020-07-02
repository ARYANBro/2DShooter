using Godot;

public class Player : KinematicBody2D
{
    [Signal] public delegate void PlayerDamaged();

    [Export] public float sprintSpeed;
    [Export] public float acceleration;
    [Export] public float sprintAccelerations;
    [Export] public float friction;
    [Export] public float speed;
    [Export]
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0)
                hp = 0;
            else if (value >= 100)
                hp = 100;
            else
                hp = value;
        }
    }
    [Export]
    public float Stamina
    {
        get
        {
            return stamina;
        }
        set
        {
            if (value <= 0)
                stamina = 0;
            else if (value >= 400)
                stamina = 400;
            else
                stamina = value;
        }
    }
    public Particles2D playerSprintParticles;
    public bool canSprint = true;
    public Vector2 velocity = Vector2.Zero;

    private int hp = 100;
    private float stamina = 400f;

    public override void _Ready()
    {
        playerSprintParticles = GetNode<Particles2D>("PlayerSpriteParitcles");
    }

    public override void _Process(float delta)
    {
        Move(delta);
        
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity);
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        EmitSignal("PlayerDamaged");
    }

    private void OnPlayerDied() => Hide();

    private void Move(float delta)
    {
        if (Input.IsActionPressed("Sprint") && canSprint && velocity != Vector2.Zero)
            Stamina -= 3;
        else
            Stamina += 1;

        if (Stamina <= 0)
            canSprint = false;
        else if (Stamina >= 400)
            canSprint = true;

        Vector2 inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
        inputVector = inputVector.Normalized();

        if (Input.IsActionPressed("Sprint") && canSprint)
        {
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * sprintSpeed, sprintAccelerations * delta);
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

            playerSprintParticles.Emitting = true;
        }
        else
        {
            playerSprintParticles.Emitting = false;
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * speed, acceleration * delta);
            else
                velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
        }
    }
}