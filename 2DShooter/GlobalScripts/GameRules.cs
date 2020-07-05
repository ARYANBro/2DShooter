using System;
using System.Collections.Generic;
using System.Security.Policy;
using Godot;

public class GameRules : Node2D
{
    [Signal] public delegate void SPlayerWon();

    [Export] public int maxNumOfEnemies;
    [Export] public int maxNumOfBigEnemies;
    [Export] public PackedScene enemyScene;
    [Export] public PackedScene bigEnemyScene;
    [Export] public PackedScene healthPackScene;
    [Export] public PackedScene energyDrinkScene;
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

        GD.Print("Enemies spawned: " + enemiesNode.GetChildCount());
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

    public override void _Process(float delta)
    {
        if (enemiesNode.GetChildCount() == 0)
        {
            EmitSignal("SPlayerWon");
        }
    }

    private void OnEnemyDied()
    {
    }

    private void OnPlayerWon()
    {
        GD.Print("Player Won");

        var healthpack = healthPackScene.Instance();
        var energyDrink = energyDrinkScene.Instance();

        GetTree().CurrentScene.CallDeferred("add_child", healthpack);
        GetTree().CurrentScene.CallDeferred("add_child", energyDrink);
    }
}