using Godot;

public class ShotgunShell : Node2D
{
    [Export] public int damage = 50;

    public void OnBulletBodyEntered(object body)
    {
        if (body is Enemy enemy)
            enemy.TakeDamage(damage);
		
        QueueFree();
    }
}
