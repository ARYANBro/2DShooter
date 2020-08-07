using Godot;
using System;
using System.Threading.Tasks;

public class RocketLauncherRocket : Node2D
{
    [Export] public int damage;
    [Export] public PackedScene hitParticlesScene;
    [Export] public NodePath DamageAreaCollisionPath;

    public Node2D hitParticleRoot;

    public GameRules gameRules;

    public override void _Ready()
    {
        hitParticleRoot = hitParticlesScene.Instance() as Node2D;
        gameRules = GetTree().CurrentScene as GameRules;
    }

    private void OnBulletBodyEntered(object body)
    {
        if (body is Enemy enemy)
            enemy.TakeDamage(damage);

        GetNode<CollisionShape2D>(DamageAreaCollisionPath).SetDeferred("disabled", false);
        hitParticleRoot.GetNode<Particles2D>("HitParticle").Emitting = true;

        hitParticleRoot.GlobalPosition = GetNode<BulletComponent>("BulletComponent").GlobalPosition;
        if (hitParticleRoot.GetParent() != GetTree().CurrentScene)
            GetTree().CurrentScene.AddChild(hitParticleRoot);

        GetTree().CurrentScene.GetNode<CameraShake>("MainCam").Shake(180, 90, 80);
        gameRules.StartSlowMotion(0.1f, 0.5f, 0.8f);

        GetTree().CreateTimer(0.1f).Connect("timeout", this, "DeleteRocket");
    }

    
    private void DeleteRocket()
    {
        GetTree().CreateTimer(1f).Connect("timeout", hitParticleRoot, "queue_free");
        QueueFree();
    }

    private void OnDamageAreaBodyEntered(object body)
    {
        if (body is Enemy enemy)
            enemy.TakeDamage(damage * 0.5f);
    }
}