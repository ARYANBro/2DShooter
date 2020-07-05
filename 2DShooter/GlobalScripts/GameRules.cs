using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    public List<Node> enemies;
    public Node2D enemiesNode;

    public override void _Ready()
    {
        enemies = new List<Node>();
        enemiesNode = GetNode<Node2D>("Enemies");
        SpawnEnemies();
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
        if (enemiesNode.GetChildCount() == 1)
            EmitSignal("SPlayerWon");
    }

    private void OnPlayerWon()
    {
        SpawnConsumables();
        GetTree().CreateTimer(5f).Connect("timeout", this, "SpawnEnemies");
    }

    private void SpawnEnemies()
    {
        enemies.Clear();
        Utlities.randNumGenerator.Randomize();
        for (int i = 0; i < Utlities.randNumGenerator.RandiRange(1, maxNumOfEnemies); i++)
        {
            Node enemy = enemyScene.Instance();
            if (i >= maxNumOfEnemies / maxNumOfBigEnemies)
                enemy = bigEnemyScene.Instance();
            enemies.Add(enemy);
        }

        for (int i = 0; i < enemies.Count; i++)
            enemiesNode.AddChild(enemies[i], true);
    }

    private void SpawnConsumables()
    {
        int randNum = Utlities.randNumGenerator.RandiRange(0, 1);
        GD.Print(randNum);

        var healthpack = healthPackScene.Instance();
        var energyDrink = energyDrinkScene.Instance();

        Action spawnHealthpack = () =>
        {
           if (healthpack.GetParent() != GetTree().CurrentScene)
            GetTree().CurrentScene.CallDeferred("add_child", healthpack, true);
        };

        Action spawnEnergydrink = () =>
        {
            if (energyDrink.GetParent() != GetTree().CurrentScene)
                GetTree().CurrentScene.CallDeferred("add_child", energyDrink, true);
        };

        if (randNum == 0)
        {
            spawnEnergydrink();
        }
        else if (randNum == 1)
        {
            spawnHealthpack();
        }
    }
}