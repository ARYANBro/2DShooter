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
    [Export] public PackedScene pointsScene;
    [Export] public PackedScene bigEnemyScene;
    [Export] public PackedScene healthPackScene;
    [Export] public PackedScene energyDrinkScene;
    [Export] public PackedScene shotGunScene;
    public Node2D enemiesNode;
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
    private bool spawnEnemies = false;
    private bool enemyCondition = false;
    static private float highScore = 0;

    private ConsumableSpawner consumableSpawner;
    private EnemySpawner enemySpawner;
    private PointsSpawner pointsSpawner;
    private GunSpawner gunSpawner;
    private Shotgun shotgun;


    public override void _Ready()
    {
        var player = GetNode<Player>("Player");
        enemiesNode = GetNode<Node2D>("Enemies");
        pauseMenue = GetNode<Control>("Hud/PauseMenue");

        gunSpawner = new GunSpawner();
        pointsSpawner = new PointsSpawner(pointsScene);
        enemySpawner = new EnemySpawner(enemyScene, bigEnemyScene, enemiesNode);
        consumableSpawner = new ConsumableSpawner(healthPackScene, energyDrinkScene, GetTree().CurrentScene);

        shotgun = (Shotgun)gunSpawner.InitializeGun(shotGunScene);
        gunSpawner.Spawn(shotgun, player.Position, player.RotationDegrees, (Node2D)GetTree().CurrentScene);

        //shotgun = shotGunScene.Instance() as Shotgun;

        pauseMenue.Visible = false;

        /* Place shotgun in the level
            if it is unlocked  */

        //if (shotgun.isUnlocked)
        //{
            //shotgun = (Shotgun)Utlities.SetNode2DParams(shotgun, player.Position, player.RotationDegrees);
            //GetTree().CurrentScene.AddChild(shotgun);
        //}


        // if (Shotgun.isUnlocked)
        // {
        //     shotgun = (Shotgun)Utlities.SetNode2DParams(shotgun, player.Position, player.RotationDegrees);
        //     GetTree().CurrentScene.AddChild(shotgun);
        // }

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
            
            /*
            if (Engine.TimeScale < 1.0f)
                GetTree().CreateTImer(2f).Connect("timeout", time, "FixEngineScale"); 
            
            */
        }

        if (Input.IsActionJustPressed("Pause"))
        {
            EmitSignal("SPauseGame");
            gameIsPaused = true;
        }

        if (Shop.currentSlot.gunName == Shop.Slot.GunNames.Shotgun && Shop.currentSlot.isUnlocked)
            shotgun.isUnlocked = true;
        
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
        if (GetTree().CurrentScene.FindNode("Shotgun") == null)
            gunSpawner.Spawn(shotgun, GetNode<Player>("Player").Position, GetNode<Player>("Player").RotationDegrees, (Node2D)GetTree().CurrentScene);
        else
            GD.Print("Shotgun is already there");
                    
        GetTree().CreateTimer(5f).Connect("timeout", this, "SpawnEnemiesTimerTimeout");
    }

    private void SpawnEnemiesTimerTimeout() => spawnEnemies = true;
    private void OnPlayerDied() => GetTree().Paused = true;
    private void SpawnPoints(Vector2 position, int _points, Vector2 size) => pointsSpawner.Spawn(position, _points, size, GetTree());

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