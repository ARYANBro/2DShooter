using Godot;
using System;

public class AcidEnemy : Enemy
{
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
    }


    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity);
    }

    public override void TakeDamage(float damage)
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
        GetTree().CreateTimer(0.1f).Connect("timeout", this, "OnHitTimerTimeout");
    }

    public override void Shoot(float delta) {}

    private void OnAcidEnemyBodyCollided(object body)
    {
        if (body is Player)
        {
            if (!player.IsAcidic)
                player.MakePlayerAcidic(10f, 10);
        }
    }
}