using Godot;

public class Player : KinematicBody2D
{
    [Signal] public delegate void PlayerDamaged();
    [Signal] public delegate void SPlayerDied();

    public Inventory inventory;
    public MovementHandler movementHandler;
    public Particles2D playerSprintParticles;
    public Healthbar healthbar;


    private bool isAcidic = false;
    private int hp = 100;

    private int takeDamageAmount;
    private float overAllDamageTime;
    private float overAllDamageStartTime;
    private bool takeDamageOverTime = false;

    private float damageTime;
    private float damageStartTime;

    [Export]
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0) hp = 0;
            else if (value >= 100) hp = 100;
            else hp = value;
        }
    }
    
    public bool IsAcidic => isAcidic;

    public override void _Ready()
    {
        playerSprintParticles = GetNode<Particles2D>("PlayerSpriteParitcles");
        inventory = GetNode<Inventory>("Inventory");
        healthbar = GetTree().CurrentScene.GetNode<Healthbar>("Hud/HealtbarRoot");

        movementHandler = new MovementHandler(400f, 70f, 200f, 250f, 500f, 350f);

        Connect("SPlayerDied", GetTree().CurrentScene, "OnPlayerDied");
    }

    public override void _Process(float delta)
    {
        movementHandler.ProcessMovement(this, delta);

        if (takeDamageOverTime)
            TakeDamageOverTime(delta);

        if (healthbar != null)
        {
            if (isAcidic)
                healthbar.isAcidic = true;
            else
                healthbar.isAcidic = false;
        }
        
    }

    public override void _PhysicsProcess(float delta)
    {
        movementHandler.ProcessPhysics(this);
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
            EmitSignal("SPlayerDied");

        EmitSignal("PlayerDamaged");
    }

    public void StartTakingDamageOverTime(int damage, float overAllDamageStartTime, float damageStartTime)
    {
        this.overAllDamageStartTime = overAllDamageStartTime;
        this.damageStartTime = damageStartTime;
        this.takeDamageAmount = damage;

        overAllDamageTime = overAllDamageStartTime;

        takeDamageOverTime = true;
    }

    private void TakeDamageOverTime(float delta)
    {
        if (damageTime <= 0)
        {
            TakeDamage(takeDamageAmount);
            damageTime = damageStartTime;
        }
        else
            damageTime -= delta;

        if (overAllDamageTime <= 0)
        {
            takeDamageOverTime = false;
            overAllDamageTime = overAllDamageStartTime;
        }
        else
            overAllDamageTime -= delta;
    }

    public void MakePlayerAcidic(float time, int damage)
    {
        isAcidic = true;
        StartTakingDamageOverTime(damage, time, 2f);

        GetTree().CreateTimer(time).Connect("timeout", this, "OnAcidicTimerTimeout");
    }

    private void OnAcidicTimerTimeout()
    {
        isAcidic = false;
    }
    
    private void OnPlayerDied()
    {
        Hide();
    }
}