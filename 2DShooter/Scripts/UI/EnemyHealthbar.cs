using Godot;

public class EnemyHealthbar : TextureProgress
{
    public Enemy enemy { get; private set; }

    public override void _Ready()
    {
        enemy = GetParent<Enemy>();
    }

    public override void _Process(float delta)
    {
        Value = Mathf.Lerp((float)Value, enemy.Hp, 0.5f);
    }
}