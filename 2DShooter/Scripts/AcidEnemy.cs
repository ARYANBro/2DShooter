using Godot;
using System;

public class AcidEnemy : Enemy
{
    public bool damagePlayerOverTime = false;
    float startTime = 3f;
    float overallStartTime = 10f;
    float time;
    float overAllTime;

    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        time = startTime;
        overAllTime = overallStartTime;
    }

    public override void _Process(float delta)
    {
        GetMovementInput(delta);

    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity);

        if (damagePlayerOverTime)
        {
            DamagePlayerOverTime(10, delta);
        }
    }

    async public override void TakeDamage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            var cameraShake = GetTree().CurrentScene.GetNode<CameraShake>("MainCam");

            cameraShake.StartShake();

            EmitSignal("SEnemyDied", 10);
            EmitSignal("SSpawnPoints", GlobalPosition, 10, new Vector2(1f, 1f));

            GetParent().QueueFree();
        }

        GetNode<Sprite>("EnemySprite").Material.Set("shader_param/hit", true);
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetNode<Sprite>("EnemySprite").Material.Set("shader_param/hit", false);
    }

    public override void Shoot(float delta) { }


    void OnAcidEnemyBodyCollided(object body)
    {
        damagePlayerOverTime = true;
        GD.Print("Body collided");
    }

    void DamagePlayerOverTime(int damage, float delta)
    {
        if (time <= 0)
        {
            player.TakeDamage(damage);

            time = startTime;
        }
        else
            time -= delta;

        if (overAllTime <= 0)
            damagePlayerOverTime = false;
        else overAllTime -= delta;

        
    }
}