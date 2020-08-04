using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    [Signal] public delegate void PlayerDamaged();
    [Signal] public delegate void SPlayerDied();

    [Export] public float speed;
    [Export] public float friction;
    [Export] public float sprintSpeed;
    [Export] public float acceleration;
    [Export] public float sprintAccelerations;
    [Export]
    public int Hp
    {
        get {
            return hp;
        }
        set {
            if (value <= 0) hp = 0;
            else if (value >= 100) hp = 100;
            else hp = value;
        }
    }
        [Export] public float Stamina
    {
        get {
            return stamina;
        }
        set {
            if (value <= 0) stamina = 0;
            else if (value >= 400) stamina = 400;
            else stamina = value;
        }
    }
    public Particles2D playerSprintParticles;
    public Inventory inventory;
    public bool canSprint = true;
    public Vector2 velocity = Vector2.Zero;
    public float startTime = 3;
    public bool takeDamageOverTime;
    float time;

    int hp = 100;
    float stamina = 400f;
    float orignalSpeed;
    float orignalSprintSpeed;

    public override void _Ready()
    {
        playerSprintParticles = GetNode<Particles2D>("PlayerSpriteParitcles");
        inventory = GetNode<Inventory>("Inventory");

        orignalSprintSpeed = sprintSpeed;
        orignalSpeed = speed;
        time = startTime;

        Connect("SPlayerDied", GetTree().CurrentScene, "OnPlayerDied");
    }

    public override void _Process(float delta)
    {
        speed = orignalSpeed;
        sprintSpeed = orignalSprintSpeed;

        if (inventory.GetChildCount() != 0)
        {
            if (inventory.GetChild(0).GetType().Name == "RocketLauncher")
            {
                speed = speed * 0.7f;
                sprintSpeed = sprintSpeed * 0.4f;
            }
            else {
                speed = orignalSpeed;
                sprintSpeed = orignalSprintSpeed;
            }
        }

        Move(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity);
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0) EmitSignal("SPlayerDied");

        EmitSignal("PlayerDamaged");
    }

    void OnPlayerDied()
    {
        Hide();
    }

    void Move(float delta)
    {
        if (Input.IsActionPressed("Sprint") && canSprint && velocity != Vector2.Zero)
            Stamina -= 3;
        else Stamina += 1;

        if (Stamina <= 0) canSprint = false;
        else if (Stamina >= 400) canSprint = true;

        Vector2 inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
        inputVector = inputVector.Normalized();

        if (Input.IsActionPressed("Sprint") && canSprint)
        {
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * sprintSpeed, sprintAccelerations * delta);
            else velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

            playerSprintParticles.Emitting = true;
        }
        else {
            playerSprintParticles.Emitting = false;
            if (inputVector != Vector2.Zero)
                velocity = velocity.MoveToward(inputVector * speed, acceleration * delta);
            else velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
        }
    }
}