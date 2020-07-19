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
    [Export] public PackedScene shotGunScene;
    public List<Node> enemies;
    public Node2D enemiesNode;
    private int waveCount = 0;
    private int points = 0;
    private bool spawnEnemies = false;
    private bool enemyCondition = false;
    static private float highScore = 0;
    public bool engineScaleCheck = false;

    public override void _Ready()
    {
        enemies = new List<Node>();
        enemiesNode = GetNode<Node2D>("Enemies");
        var player = GetNode<Player>("Player");
        var shotgun = shotGunScene.Instance() as Shotgun;

        /* Place shotgun in the level
            if it is unlocked  */
        if (Shotgun.isUnlocked)
        {
            shotgun = (Shotgun)Utlities.SetNode2DParams(shotgun, player.Position, player.RotationDegrees);
            GetTree().CurrentScene.AddChild(shotgun);
        }

        if (player != null)
            player.Connect("SPlayerDied", this, "OnPlayerDied");

        SpawnEnemies();
    }

    // Will remove later just for testing
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
        /* Spawn enemies if the spawn condition is true
           i didn't have a better name for this */
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

        // Check if Engine scale is ok
        if (engineScaleCheck == true)
        {
            if (Engine.TimeScale < 1.0f)
                GetTree().CreateTimer(2f).Connect("timeout", this, "OnEnginScaleFixTimerFixTimeout");
        }
    }

    void OnEnginScaleFixTimerFixTimeout()
    {
        Engine.TimeScale = 1.0f;

        engineScaleCheck = false;
    }

    private void OnEnemyDied(int _points)
    {
        /* Spawn more enemies
           and Increase points */
        enemyCondition = true;
        EmitSignal("SIncreasePoints", _points);
        points += _points;

        if (highScore < points)
            highScore = points;
    }

    private void OnPlayerWon()
    {
        // Spawn Consumables and Enemies
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
        // Randomly spawn enemeis
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

        // Randomly spawn
        if (randNum == 0)
        {
            if (energyDrink.GetParent() != GetTree().CurrentScene)
                GetTree().CurrentScene.CallDeferred("add_child", energyDrink, true);
        }
        else if (randNum == 1)
        {
            if (healthpack.GetParent() != GetTree().CurrentScene)
                GetTree().CurrentScene.CallDeferred("add_child", healthpack, true);
        }
    }

    private void SpawnPoints(Vector2 position, int _points, Vector2 size)
    {
        // On top of enemies
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