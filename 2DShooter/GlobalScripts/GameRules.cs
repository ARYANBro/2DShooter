using Godot;
using System;
using System.Collections.Generic;

public class GameRules : Node2D
{
    [Signal] public delegate void SPlayerWon();
    [Signal] public delegate void SIncreasePoints(int points);

    [Export] public int maxEnemyCount;
    [Export] public int maxBigEnemyCount;
    [Export] public PackedScene enemyScene;
    [Export] public PackedScene pointsScene;
    [Export] public PackedScene shotGunScene;
    [Export] public PackedScene bigEnemyScene;
    [Export] public PackedScene healthPackScene;
    [Export] public PackedScene energyDrinkScene;
    [Export] public PackedScene rocketLauncherScene;
    public Node2D enemiesNode;
    public Player player;
    public Control pauseMenue;
    public static bool gameIsPaused;
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

    private int points = 0;
    private bool enemyCondition = false;
    static private float highScore = 0;
    private Shotgun shotgun;
    private RocketLauncher rocketLauncher;
    private GunSpawner gunSpawner;
    private EnemySpawner enemySpawner;
    private PointsSpawner pointsSpawner;
    private ConsumableSpawner consumableSpawner;

    public override void _Ready()
    {
        player = GetNode<Player>("Player");
        enemiesNode = GetNode<Node2D>("Enemies");
        pauseMenue = GetNode<Control>("Hud/PauseMenue");

        player.Connect("SPlayerDied", this, "OnPlayerDied");

        pauseMenue.Visible = false;

        gunSpawner = new GunSpawner();
        pointsSpawner = new PointsSpawner(pointsScene);
        enemySpawner = new EnemySpawner(enemyScene, bigEnemyScene, enemiesNode);
        consumableSpawner = new ConsumableSpawner(healthPackScene, energyDrinkScene, GetTree().CurrentScene);

        // Spawn Guns
        rocketLauncher = gunSpawner.InitGun(rocketLauncherScene) as RocketLauncher;
        shotgun = gunSpawner.InitGun(shotGunScene) as Shotgun;
        gunSpawner.Spawn<Shotgun>(ref shotgun, player.Position, player.RotationDegrees, GetTree().CurrentScene);
        gunSpawner.Spawn<RocketLauncher>(ref rocketLauncher, player.Position, player.RotationDegrees, GetTree().CurrentScene);

        // Spawn Enemies
        enemySpawner.SpawnEnemies = true;
        enemySpawner.SetSpawnParams(maxEnemyCount, maxBigEnemyCount);
    }

    public override void _Process(float delta)
    {
        enemySpawner.SetSpawnParams(maxEnemyCount, maxBigEnemyCount);
        GamePauseStateCheck();
        EngineScaleCheck();
        PlayerWonCheck();

        if (Input.IsActionJustPressed("Pause"))
            gameIsPaused = true;

        gunSpawner.GunUnlockCheck<Shotgun>(ref shotgun, Shop.Slot.GunNames.Shotgun);
        gunSpawner.GunUnlockCheck<RocketLauncher>(ref rocketLauncher, Shop.Slot.GunNames.RocketLauncher);
    }

    private void OnEnemyDied(int _points)
    {
        // Spawn more enemies
        enemyCondition = true;

        EmitSignal("SIncreasePoints", _points);
        points += _points;

        // Increase points
        if (highScore < points)
            highScore = points;
    }

    private void OnPlayerWon()
    {
        // Spawn Consumables and Enemies
        consumableSpawner.Spawn();
        gunSpawner.Spawn<Shotgun>(ref shotgun, player.Position, player.RotationDegrees, GetTree().CurrentScene);
        gunSpawner.Spawn<RocketLauncher>(ref rocketLauncher, player.Position, player.RotationDegrees, GetTree().CurrentScene);

        GetTree().CreateTimer(5f).Connect("timeout", this, "SpawnEnemiesTimerTimeout");
    }

    private void SpawnEnemiesTimerTimeout()
    {
        enemySpawner.SpawnEnemies = true;
    }

    private void OnPlayerDied() => GetTree().Quit();
    private void SpawnPoints(Vector2 position, int _points, Vector2 size) => pointsSpawner.Spawn(position, _points, size, GetTree());

    private void FixEngineScale()
    {
        Engine.TimeScale = 1.0f;
        engineScaleCheck = false;
    }

    public void PauseGame()
    {
        Engine.TimeScale = 0.0f;
        GetTree().CurrentScene.GetNode<Control>("Hud/PauseMenue").Visible = true;
    }

    public static void ResumeGame()
    {
        Engine.TimeScale = 1.0f;
    }

    private void EngineScaleCheck()
    {
        // Check if Engine scale is ok
        if (engineScaleCheck == true)
        {
            if (Engine.TimeScale < 1.0f)
                GetTree().CreateTimer(2f).Connect("timeout", this, "FixEngineScale");
        }
    }

    private void GamePauseStateCheck()
    {
        if (gameIsPaused)
            PauseGame();
        else ResumeGame();
    }

    private void PlayerWonCheck()
    {
        if (enemyCondition == true)
        {
            if (enemiesNode.GetChildCount() == 0)
            {
                EmitSignal("SPlayerWon");
                enemyCondition = false;
            }
        }
    }

}