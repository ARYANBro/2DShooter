using System;
using Godot;

public class GameRules : Node2D
{
    private void OnPlayerDied()
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