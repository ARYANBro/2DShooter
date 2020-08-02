using Godot;
using System;

public class PauseMenue : Control
{
    public override void _Ready()
    {
        Visible = false;
    }

    void OnResumeButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().CurrentScene.GetNode<TextureButton>("Hud/PauseMenue/PauseButton").Visible = true;
        Hide(); 
    }


    void OnShopButtonPressed()
    {
        GetTree().ChangeScene("res://Assets/Shop.tscn");
    }
}
