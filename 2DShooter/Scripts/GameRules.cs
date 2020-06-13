using System;
using Godot;

public class GameRules : Node2D
{
    public Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
    }

    private void OnPlayerDied()
    {
        timer.Start();
    }

    private void OnTimertimeout()
    {
        GetTree().Paused = true;
    }

    public override void _Process(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.Escape))
        {
            GetTree().Quit();
        }
    }
}