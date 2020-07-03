using System;
using System.Collections.Generic;
using Godot;

public class GameRules : Node2D
{
    [Export] public int maxNumOfEnemies;
    [Export] public int maxNumOfBigEnemies;
    [Export] public PackedScene enemyScene;
    [Export] public PackedScene bigEnemyScene;
    public List<Node> enemies = new List<Node>();
    public Node2D enemiesNode;

    public override void _Ready()
    {
        enemiesNode = GetNode<Node2D>("Enemies");
        
        for (int i = 0; i < Utlities.randNumGenerator.RandiRange(1, maxNumOfEnemies); i++)
        {
            Node enemy = enemyScene.Instance();
            if (i >= maxNumOfEnemies / maxNumOfBigEnemies)
                enemy = bigEnemyScene.Instance();
            enemies.Add(enemy);
        }

        for (int i = 0; i < enemies.Count; i++)
            enemiesNode.AddChild(enemies[i], true);

        GD.Print("Enemies spawned: " + enemiesNode.GetChildCount()) ;
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
        if (enemiesNode.GetChildCount() - 1 == 1)
            GD.Print("You Win");
    }
}