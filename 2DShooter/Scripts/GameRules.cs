using System;
using Godot;

// Game Rules
public class GameRules : Node2D
{
    private void OnPlayerDied()
    {
        GetTree().Paused = true;
    }

    public override void _Process(float delta)
    {
        // Input just for testing. 

        // Quitting the game.
        if (Input.IsKeyPressed((int)KeyList.Escape))
        {
            GetTree().Quit();
        }

        // Fullscreen and widowed mode.
        if (Input.IsKeyPressed((int)KeyList.F11) && OS.WindowFullscreen)
        {
            OS.WindowFullscreen = false;
        }
        else if (Input.IsKeyPressed((int)KeyList.F11) && !OS.WindowFullscreen)
        {
            OS.WindowFullscreen = true;
        }
    }
}