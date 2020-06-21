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
        if (Input.IsActionJustPressed("ui_cancel") && !OS.WindowFullscreen)
        {
            OS.WindowFullscreen = false;
        }
        else if (Input.IsActionJustPressed("ui_cancel") && OS.WindowFullscreen)
        {
            OS.WindowFullscreen = true;
        }


    }
}