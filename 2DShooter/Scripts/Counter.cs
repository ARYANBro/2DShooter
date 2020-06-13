using System;
using Godot;

public class Counter : Label
{
    private int counter = 0;

    public override void _Ready()
    {
        var childrens = GetTree().CurrentScene.GetChildren();

        foreach (var children in childrens)
        {
            if (children.GetType().Name == "Enemy")
            {
                Enemy enemy = children as Enemy;
                enemy.Connect("EnemyDiedSignal", this, "OnEnemyDied");
            }
        }
    }

    private void OnEnemyDied()
    {
        counter += 10;
        Text = Convert.ToString(counter);
    }
}
