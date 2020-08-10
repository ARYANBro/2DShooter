using Godot;
using System;

public class DeathScreen : Control
{
    private void OnExitButtonPressed()
    {
        GetTree().Quit(0);
    }

    private void OnRetryButtonPressed()
    {
        GetTree().ReloadCurrentScene();
    }
}
