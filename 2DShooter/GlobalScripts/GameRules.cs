using System;
using System.Linq;
using Godot;

public class GameRules : Node2D
{
    [Export] public int maxNumOfEnemies;
    [Export] public int maxNumOfBigEnemies;
    [Export] public PackedScene enemyScene;
    [Export] public PackedScene bigEnemyScene;

    public override void _Ready()
    {

        for (int i = 0; i < Utlities.randNumGenerator.RandiRange(1, maxNumOfEnemies); i++)
        {
            Node enemy = enemyScene.Instance();
            if (i >= maxNumOfEnemies / maxNumOfBigEnemies)
                enemy = bigEnemyScene.Instance();

            AddChild(enemy);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Scancode == (uint)KeyList.R)
                GetTree().ReloadCurrentScene();
        }
    }
    private void OnPlayerDied() => GetTree().Quit();

    private void OnEnemyDied()
    {
        GD.Print("Enemy Died");
    }
}