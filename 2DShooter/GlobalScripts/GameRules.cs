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
    [Export] public PackedScene pointsScene;
    public List<Node> enemies;
    public Node2D enemiesNode;
    private int waveCount = 0;
    private int points = 0;

    private bool spawnEnemies = false;
    private bool enemyCondition = false;
    public bool engineScaleCheck = false;

    public override void _Ready()
    {
        enemies = new List<Node>();
        enemiesNode = GetNode<Node2D>("Enemies");
        var player = GetNode<Player>("Player");
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

    public override void _Process(float delta)
    {
        if (spawnEnemies == true)
            SpawnEnemies();

        if (enemyCondition == true)
        {
            if (enemiesNode.GetChildCount() == 0)
            {
                enemies.Clear();
                EmitSignal("SPlayerWon");
                enemyCondition = false;
            }
        }

        if (engineScaleCheck == true)
        {
            if (Engine.TimeScale < 1.0f)
            {
                GetTree().CreateTimer(2f).Connect("timeout", this, "OnEnginScaleFixTimerFixTimeout");
            }
        }

    }

    void OnEnginScaleFixTimerFixTimeout()
    {
        Engine.TimeScale = 1.0f;

        engineScaleCheck = false;
    }

    private void OnEnemyDied(int _points)
    {
        enemyCondition = true;
        EmitSignal("SIncreasePoints", _points);
    }

    private void OnPlayerWon()
    {
        SpawnConsumables();
        GetTree().CreateTimer(5f).Connect("timeout", this, "OnTimerTimeoutEnemiesSpawn");
    }

    private void OnTimerTimeoutEnemiesSpawn() => spawnEnemies = true;

    private void OnPlayerDied()
    {
        GetTree().Paused = true;
    }

    private void SpawnEnemies()
    {
        waveCount++;
        enemies.Clear();
        Utlities.randNumGenerator.Randomize();
        for (int i = 0; i < Utlities.randNumGenerator.RandiRange(1, maxNumOfEnemies); i++)
        {
            var enemy = enemyScene.Instance();
            if (i >= maxNumOfEnemies / maxNumOfBigEnemies)
                enemy = bigEnemyScene.Instance();
            enemies.Add(enemy);
        }

        for (int i = 0; i < enemies.Count; i++)
            enemiesNode.AddChild(enemies[i], true);

        spawnEnemies = false;
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

    private void SpawnPoints(Vector2 position, int _points, Vector2 size)
    {
        var points = pointsScene.Instance() as Node2D;
        var pointsLabel = points.GetNode<Label>("PointsAnimPlayer/Points");
        var pointsNode2D = points.GetNode<Node2D>("PointsAnimPlayer");

        pointsLabel.RectPosition = position;
        pointsLabel.RectScale = size;
        pointsLabel.Text = "+" + Convert.ToString(_points);

        GetTree().CurrentScene.AddChild(points, true);
        GetTree().CreateTimer(1f).Connect("timeout", points, "queue_free");
    }
}