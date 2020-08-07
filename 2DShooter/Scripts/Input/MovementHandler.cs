using Godot;
using System;

public class MovementHandler : Node
{
    public float accel;
    public float speed;
    public float friction;
    public float sprintAccel;
    public float sprintSpeed;

    public bool canSprint = true;
    public Vector2 velocity = new Vector2();

    float stamina = 400f;
    float orignalSpeed;
    float orignalSprintSpeed;

    private InputHandler inputHandler = new InputHandler();

    public MovementHandler()
    {
        this.accel = 0;
        this.speed = 0;
        this.stamina = 0;
        this.friction = 0;
        this.sprintAccel = 0;
        this.sprintSpeed = 0;

        orignalSpeed = speed;
        orignalSprintSpeed = sprintSpeed;
    }

    public MovementHandler(float stamina, float speed, float friction, float sprintSpeed, float accel, float sprintAccel)
    {
        this.accel = accel;
        this.speed = speed;
        this.stamina = stamina;
        this.friction = friction;
        this.sprintAccel = sprintAccel;
        this.sprintSpeed = sprintSpeed;

        orignalSpeed = speed;
        orignalSprintSpeed = sprintSpeed;
    }

    public float Stamina
    {
        get
        {
            return stamina;
        }
        set
        {
            if (value <= 0) stamina = 0;
            else if (value >= 400) stamina = 400;
            else stamina = value;
        }
    }

    public void ProcessMovement(Player player, float delta)
    {
        if (player.inventory.Has<RocketLauncher>())
        {
            speed = 45f;
            sprintSpeed = 125f;
        }
        else
        {
            speed = orignalSpeed;
            sprintSpeed = orignalSprintSpeed;
        }

        if (inputHandler.SprintPressed(this))
            Sprint(player, delta);
        else
            Walk(player, delta);

        SprintCheck();
    }

    public void ProcessPhysics(Player player)
    {
        velocity = player.MoveAndSlide(velocity);
    }

    private bool SprintCheck()
    {
        if (Stamina <= 0)
        {
            canSprint = false;
            return false;
        }
        else if (Stamina >= 400)
        {
            canSprint = true;
            return true;
        }
        else
            return false;
    }

    private void Sprint(Player player, float delta)
    {
        Vector2 inputVector = inputHandler.GetInputVector();
        if (inputVector != Vector2.Zero)
            velocity = velocity.MoveToward(inputVector * sprintSpeed, sprintAccel * delta);
        else
            velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

        Stamina -= 3;
        player.playerSprintParticles.Emitting = true;
    }

    private void Walk(Player player, float delta)
    {
        Vector2 inputVector = inputHandler.GetInputVector();
        if (inputVector != Vector2.Zero)
            velocity = velocity.MoveToward(inputVector * speed, accel * delta);
        else
            velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

        player.playerSprintParticles.Emitting = false;
        Stamina += 1;
    }
}