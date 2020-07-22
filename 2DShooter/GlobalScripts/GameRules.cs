using Godot;
using System;
using System.Collections.Generic;

public class GameRules : Node2D
{
    [Signal] public delegate void SPlayerWon();
    [Signal] public delegate void SPauseGame();
    [Signal] public delegate void SIncreasePoints(int points);

    [Export] public int maxEnemyCount;
    [Export] public int maxBigEnemyCount;
    [Export] public PackedScene enemyScene;
    [Export] public PackedScene bigEnemyScene;
    [Export] public PackedScene healthPackScene;
    [Export] public PackedScene energyDrinkScene;
    [Export] public PackedScene pointsScene;
    [Export] public PackedScene shotGunScene;
    public Node2D enemiesNode;
    public Control pauseMenue;  

    private int points = 0;
    private bool spawnEnemies = false;
    private bool enemyCondition = false;
    static private float highScore = 0;
    public bool engineScaleCheck = false;

    public static float HighScore
    {
        get
        {
            return highScore;
        }

        private set
        {
           highScore = value; 
        }
    }

    private ConsumableSpawner consumableSpawner;
    private EnemySpawner enemySpawner;
    private PointsSpawner pointsSpawner;
    public static bool gameIsPaused;


    public override void _Ready()
    {
        var player = GetNode<Player>("Player");
        enemiesNode = GetNode<Node2D>("Enemies");
        pauseMenue = GetNode<Control>("Hud/PauseMenue");

        consumableSpawner = new ConsumableSpawner(healthPackScene, energyDrinkScene, GetTree().CurrentScene);
        enemySpawner = new EnemySpawner(enemyScene, bigEnemyScene, enemiesNode);
        pointsSpawner = new PointsSpawner(pointsScene);

        var shotgun = shotGunScene.Instance() as Shotgun;

        pauseMenue.Visible = false;

        /* Place shotgun in the level
            if it is unlocked  */
        if (Shotgun.isUnlocked)
        {
            shotgun = (Shotgun)Utlities.SetNode2DParams(shotgun, player.Position, player.RotationDegrees);
            GetTree().CurrentScene.AddChild(shotgun);
        }

        if (player != null)
            player.Connect("SPlayerDied", this, "OnPlayerDied");

        enemySpawner.Spawn(maxEnemyCount, maxBigEnemyCount, ref spawnEnemies);  
    }

    public override void _Process(float delta)
    {
        /* Spawn enemies if the spawn condition is true
           i didn't have a better name for this*/
        if (spawnEnemies == true)
            enemySpawner.Spawn(maxEnemyCount, maxBigEnemyCount, ref spawnEnemies);

        if (gameIsPaused)
            PauseGame();
        else ResumeGame();

        if (enemyCondition == true)
        {
            if (enemiesNode.GetChildCount() == 0)
            {
                EmitSignal("SPlayerWon");
                enemyCondition = false;
            }
        }

        // Check if Engine scale is ok
        if (engineScaleCheck == true)
        {
            if (Engine.TimeScale < 1.0f)
                GetTree().CreateTimer(2f).Connect("timeout", this, "FixEngineScale");
        }

        if (Input.IsActionJustPressed("Pause"))
        {
            EmitSignal("SPauseGame");
            gameIsPaused = true;
        }
    }

    void FixEngineScale()
    {
        Engine.TimeScale = 1.0f;
        engineScaleCheck = false;
    }

    private void OnEnemyDied(int _points)
    {
        // Spawn more enemies and Increase points
        enemyCondition = true;
        EmitSignal("SIncreasePoints", _points);
        points += _points;

        if (highScore < points)
            highScore = points;
    }

    private void OnPlayerWon()
    {
        // Spawn Consumables and Enemies
        consumableSpawner.Spawn();
        GetTree().CreateTimer(5f).Connect("timeout", this, "SpawnEnemiesTimerTimeout");
    }

    private void SpawnEnemiesTimerTimeout() => spawnEnemies = true;
    private void OnPlayerDied() => GetTree().Paused = true;
    private void SpawnPoints(Vector2 position, int _points, Vector2 size) => pointsSpawner.Spawn(position, points, size, GetTree());

    public void PauseGame()
    {
        Engine.TimeScale = 0.0f;
        ShowPauseMenue();   
    }

    public static void ResumeGame()
    {
        Engine.TimeScale = 1.0f;
    }

    public void ShowPauseMenue()
    {
        GetTree().CurrentScene.GetNode<Control>("Hud/PauseMenue").Visible = true;
    }
}