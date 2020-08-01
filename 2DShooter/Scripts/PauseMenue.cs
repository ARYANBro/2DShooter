using Godot;
using System;

public class PauseMenue : Control
{
    public override void _Ready()
    {
        Visible = false;
    }

    private void OnShopButtonPressed()
    {
        GetTree().ChangeScene("res://Assets/Shop.tscn");
    }

    private void OnResumeButtonPressed()
    {
        GameRules.gameIsPaused = false;
        Hide(); 
    }
}
