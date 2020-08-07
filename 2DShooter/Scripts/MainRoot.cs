using Godot;
using System;

public class MainRoot : GameRules
{
    [Signal] public delegate void SPlayerWon();
    [Signal] public delegate void SIncreasePoints(int points);

    [Export] public int enemyCount;
    [Export] public int bigEnemyCount;
    [Export] public int acidEnemyCount;

    [Export] public PackedScene pointsScene;
    [Export] public PackedScene pistolScene;
    [Export] public PackedScene shotGunScene;
    [Export] public PackedScene rocketLauncherScene;

    [Export] public PackedScene enemyScene;
    [Export] public PackedScene bigEnemyScene;
    [Export] public PackedScene acidEnemyScene;

    [Export] public PackedScene healthPackScene;
    [Export] public PackedScene energyDrinkScene;

    public Player player;
    public Node2D enemiesNode;
    public Control pauseMenue;

    private int points = 0;
    private bool hasPlayerWonCheck = false;
    private static float highScore = 0;

    private Pistol pistol;
    private Shotgun shotgun;
    private RocketLauncher rocketLauncher;

    private Healthpack healthpack;
    private EnergyDrink energyDrink;

    private GunSpawner gunSpawner;
    private EnemySpawner enemySpawner;
    private PointsSpawner pointsSpawner;
    private ConsumableSpawner consumableSpawner;
    
    private InputHandler inputHandler;

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

    public override void _Ready()
    {
        player = GetNode<Player>("Player");
        enemiesNode = GetNode<Node2D>("Enemies");
        pauseMenue = GetNode<Control>("Hud/PauseMenue/PauseMenue");

        gunSpawner = new GunSpawner();
        inputHandler = new InputHandler();
        enemySpawner = new EnemySpawner(enemiesNode);
        pointsSpawner = new PointsSpawner(pointsScene);
        consumableSpawner = new ConsumableSpawner(healthPackScene, energyDrinkScene, GetTree().CurrentScene);

        // Spawn Guns
        gunSpawner.InitGun<Pistol>(ref pistol, pistolScene);
        gunSpawner.InitGun<Shotgun>(ref shotgun, shotGunScene);
        gunSpawner.InitGun<RocketLauncher>(ref rocketLauncher, rocketLauncherScene);

        SpawnGuns();
        SpawnEnemies();
    }

    public override void _Process(float delta)
    {
        EngineScaleCheck();
        PlayerWonCheck();

        if (enableSlowMo)
            SlowMo(delta);

        if (inputHandler.PausedPressed() && !GetTree().Paused)
            PauseGame();

        gunSpawner.GunUnlockCheck<Pistol>(ref pistol, 0);
        gunSpawner.GunUnlockCheck<Shotgun>(ref shotgun, 1);
        gunSpawner.GunUnlockCheck<RocketLauncher>(ref rocketLauncher, 2);

        if (inputHandler.FullScreenPressed() && !OS.WindowFullscreen)
            OS.WindowFullscreen = true;
        else if (inputHandler.FullScreenPressed() && OS.WindowFullscreen)
            OS.WindowFullscreen = false;
    }

    private void OnEnemyDied(int _points)
    {
        // Increase points
        hasPlayerWonCheck = true;

        EmitSignal("SIncreasePoints", _points);
        points += _points;

        if (highScore < points)
            highScore = points;
    }

    private void OnPlayerWon()
    {
        // Spawn Consumables and Enemies
        SpawnConsumables();
        SpawnGuns();

        GetTree().CreateTimer(5f).Connect("timeout", this, "SpawnEnemies");
    }

    private void OnPlayerDied()
    {
        GetTree().Quit();
    }

    private void SpawnPoints(Vector2 position, int _points, Vector2 size)
    {
        pointsSpawner.Spawn(position, _points, size, GetTree());
    }

    private void PlayerWonCheck()
    {
        if (hasPlayerWonCheck == true)
        {
            if (enemiesNode.GetChildCount() == 0)
            {
                EmitSignal("SPlayerWon");
                hasPlayerWonCheck = false;
            }
        }
    }

    private void SpawnEnemies()
    {
        Utlities.random.Randomize();

        int randomEnemyNum = Utlities.random.RandiRange(0, enemyCount);
        int randomBigEnemyNum = Utlities.random.RandiRange(0, bigEnemyCount);
        int randomAcidEnemyNum = Utlities.random.RandiRange(0, acidEnemyCount);

        enemySpawner.Spawn(enemyScene, randomEnemyNum);
        enemySpawner.Spawn(bigEnemyScene, randomBigEnemyNum);
        enemySpawner.Spawn(acidEnemyScene, randomAcidEnemyNum);
    }

    private void SpawnGuns()
    {
        gunSpawner.Spawn<Pistol>(ref pistol, player.Position, player.RotationDegrees, GetTree().CurrentScene);
        gunSpawner.Spawn<Shotgun>(ref shotgun, player.Position, player.RotationDegrees, GetTree().CurrentScene);
        gunSpawner.Spawn<RocketLauncher>(ref rocketLauncher, player.Position, player.RotationDegrees, GetTree().CurrentScene);
    }

    // For spawning consumables
    private void SpawnConsumables()
    {
        healthpack = consumableSpawner.InitConsumables(healthPackScene) as Healthpack;
        energyDrink = consumableSpawner.InitConsumables(energyDrinkScene) as EnergyDrink;

        int randNumber = Utlities.random.RandiRange(0, 1);

        if (randNumber == 1)
        {
            Vector2 resolution = GetTree().Root.GetVisibleRect().Size;
            Vector2 position = new Vector2(Utlities.random.RandfRange(0, resolution.x), Utlities.random.RandfRange(0, resolution.y));

            consumableSpawner.Spawn<Healthpack>(ref healthpack, position, 0f, GetTree().CurrentScene);
            energyDrink.QueueFree();
        }
        else
        {
            Vector2 resolution = GetTree().Root.GetVisibleRect().Size;
            Vector2 position = new Vector2(Utlities.random.RandfRange(0, resolution.x), Utlities.random.RandfRange(0, resolution.y));

            consumableSpawner.Spawn<EnergyDrink>(ref energyDrink, position, 0f, GetTree().CurrentScene);
            healthpack.QueueFree();
        }
    }

    private void OnPauseButtonPressed()
    {
        PauseGame();
    }
}