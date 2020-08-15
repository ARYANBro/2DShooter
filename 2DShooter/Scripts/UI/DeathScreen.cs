using Godot;

public class DeathScreen : Control
{   
    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnRetryButtonPressed()
    {
        GetTree().ReloadCurrentScene();
    }
}
