using Godot;
using System;
using System.Collections.Generic;

public class GameRules : Node2D
{
    [Signal] public delegate void SPlayerWon();
    [Signal] public delegate void SIncreasePoints(int points);

    [Export] public int maxNumOfEnemies;
    [Export] public int maxNumOfBigEnemies;
    [Export] public PackedScene enemyScene;
    [Export] public PackedScene bigEnemyScene;
    [Export] public PackedScene healthPackScene;
    [Export] public PackedScene energyDrinkScene;
    [Export] public PackedScene enemyPointsSpawnScene;
    public List<Node> enemies;
    public Node2D enemiesNode;

    private int waveCount = 0;
    private int points = 0;
    // private int highScore;

    public override void _Ready()
    {
        enemies = new List<Node>();
        enemiesNode = GetNode<Node2D>("Enemies");
        Player player = GetNode<Player>("Player");
        if (player != null)
            player.Connect("SPlayerDied", this, "OnPlayerDied");

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

    private void OnEnemyDied(int _points)
    {
        enemies.RemoveAt(0);
        if (enemies.Count == 0) EmitSignal("SPlayerWon");

        EmitSignal("SIncreasePoints", _points);
        //points += points;
    }

    private void OnPlayerWon()
    {
        SpawnConsumables();
        GetTree().CreateTimer(5f).Connect("timeout", this, "SpawnEnemies");
    }

    private void OnPlayerDied()
    {
        // Player Died
    }

    private void SpawnEnemies()
    {
        waveCount++;
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

        if (randNum == 0) spawnEnergydrink();
        else if (randNum == 1) spawnHealthpack();
    }

    private void SpawnPoints(Vector2 position)
    {
        Node2D enemyPointsSpawn = enemyPointsSpawnScene.Instance() as Node2D;
        enemyPointsSpawn.GlobalPosition = position;
        var animPlayer = enemyPointsSpawn.GetNode<AnimationPlayer>("PointsAnimPlayer");
        animPlayer.Play("PointsAnim");
        GetTree().CurrentScene.AddChild(enemyPointsSpawn, true);  
    }
}