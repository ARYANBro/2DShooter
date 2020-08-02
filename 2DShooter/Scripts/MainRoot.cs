using Godot;
using System;
using System.Collections.Generic;

public class MainRoot : GameRules
{
    [Signal] public delegate void SPlayerWon();
    [Signal] public delegate void SIncreasePoints(int points);

    [Export] public int maxEnemyCount;
    [Export] public int maxBigEnemyCount;
    [Export] public PackedScene enemyScene;
    [Export] public PackedScene pointsScene;
    [Export] public PackedScene pistolScene;
    [Export] public PackedScene shotGunScene;
    [Export] public PackedScene bigEnemyScene;
    [Export] public PackedScene healthPackScene;
    [Export] public PackedScene energyDrinkScene;
    [Export] public PackedScene rocketLauncherScene;
    public Player player;
    public Node2D enemiesNode;
    public Control pauseMenue;

    public static float HighScore
    {
        get {
            return highScore;
        }

        private set
        {
            highScore = value;
        }
    }

    int points = 0;
    bool enemyCondition = false;
    static float highScore = 0;
    Pistol pistol;
    Shotgun shotgun;
    Healthpack healthpack;
    GunSpawner gunSpawner;
    EnergyDrink energyDrink;
    EnemySpawner enemySpawner;
    PointsSpawner pointsSpawner;
    RocketLauncher rocketLauncher;
    ConsumableSpawner consumableSpawner;

    public override void _Ready()
    {
        player = GetNode<Player>("Player");
        enemiesNode = GetNode<Node2D>("Enemies");
        pauseMenue = GetNode<Control>("Hud/PauseMenue/PauseMenue");

        gunSpawner = new GunSpawner();
        pointsSpawner = new PointsSpawner(pointsScene);
        enemySpawner = new EnemySpawner(enemyScene, bigEnemyScene, enemiesNode);
        consumableSpawner = new ConsumableSpawner(healthPackScene, energyDrinkScene, GetTree().CurrentScene);

        // Spawn Guns
        gunSpawner.InitGun<Pistol>(ref pistol, pistolScene);
        gunSpawner.InitGun<Shotgun>(ref shotgun, shotGunScene);
        gunSpawner.InitGun<RocketLauncher>(ref rocketLauncher, rocketLauncherScene);
        
        gunSpawner.Spawn<Shotgun>(ref shotgun, player.Position, player.RotationDegrees, GetTree().CurrentScene);
        gunSpawner.Spawn<RocketLauncher>(ref rocketLauncher, player.Position, player.RotationDegrees, GetTree().CurrentScene);
        gunSpawner.Spawn<Pistol>(ref pistol,  player.Position, player.RotationDegrees, GetTree().CurrentScene);

        // Spawn Enemies
        enemySpawner.Spawn(maxEnemyCount, maxBigEnemyCount);
    }

    public override void _Process(float delta)
    {
        EngineScaleCheck();
        PlayerWonCheck();

        if (Input.IsActionJustPressed("Pause") && !GetTree().Paused)
            PauseGame();

        gunSpawner.GunUnlockCheck<Pistol>(ref pistol, 0);
        gunSpawner.GunUnlockCheck<Shotgun>(ref shotgun, 1);
        gunSpawner.GunUnlockCheck<RocketLauncher>(ref rocketLauncher, 2);

        if (Input.IsActionJustPressed("FullScreen") && !OS.WindowFullscreen)
            OS.WindowFullscreen = true;
        else if (Input.IsActionJustPressed("FullScreen") && OS.WindowFullscreen)
            OS.WindowFullscreen = false;
    }

    void OnEnemyDied(int _points)
    {
        // Spawn more enemies
        enemyCondition = true;

        EmitSignal("SIncreasePoints", _points);
        points += _points;

        // Increase points
        if (highScore < points)
            highScore = points;
    }

    void OnPlayerWon()
    {
        // Spawn Consumables and Enemies
        SpawnConsumables();
        gunSpawner.Spawn<Shotgun>(ref shotgun, player.Position, player.RotationDegrees, GetTree().CurrentScene);
        gunSpawner.Spawn<RocketLauncher>(ref rocketLauncher, player.Position, player.RotationDegrees, GetTree().CurrentScene);

        GetTree().CreateTimer(5f).Connect("timeout", this, "SpawnEnemiesTimerTimeout");
    }

    void SpawnEnemiesTimerTimeout()
    {
        enemySpawner.Spawn(maxEnemyCount, maxBigEnemyCount);
    }

    void OnPlayerDied() => GetTree().Quit();
    void SpawnPoints(Vector2 position, int _points, Vector2 size)
    {
        pointsSpawner.Spawn(position, _points, size, GetTree());
    }
 
    void PlayerWonCheck()
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

    void SpawnConsumables()
    {
        healthpack = consumableSpawner.InitConsumables(healthPackScene) as Healthpack;
        energyDrink = consumableSpawner.InitConsumables(energyDrinkScene) as EnergyDrink;

        int randNumber = Utlities.randNumGenerator.RandiRange(0, 1);

        if (randNumber == 1)
        {
            Vector2 resolution = GetTree().Root.GetVisibleRect().Size;
            Vector2 position = new Vector2(Utlities.randNumGenerator.RandfRange(0, resolution.x), Utlities.randNumGenerator.RandfRange(0, resolution.y));

            consumableSpawner.Spawn<Healthpack>(ref healthpack, position, 0f, GetTree().CurrentScene);
            energyDrink.QueueFree();
        }
        else {
            Vector2 resolution = GetTree().Root.GetVisibleRect().Size;
            Vector2 position = new Vector2(Utlities.randNumGenerator.RandfRange(0, resolution.x), Utlities.randNumGenerator.RandfRange(0, resolution.y));

            consumableSpawner.Spawn<EnergyDrink>(ref energyDrink, position, 0f, GetTree().CurrentScene);
            healthpack.QueueFree();
        }
    }

    void OnPauseButtonPressed()
    {
        PauseGame();
    }
}   