using Godot;
using System;

public class PauseMenue : Control
{
    private MainRoot main;
    private HBoxContainer buttons;
    private TextureButton pauseButton;

    public override void _Ready()
    {
        main = GetTree().CurrentScene as MainRoot;
        pauseButton = GetNode<TextureButton>("PauseButton");
        buttons = GetNode<HBoxContainer>("Buttons");
     
        buttons.Hide();
    }

    private void OnResumeButtonPressed()
    {
        main.ResumeGame();
        
        buttons.Visible = false;
        pauseButton.Visible = true; 
    }


    private void OnShopButtonPressed()
    {
        GetTree().ChangeScene("res://Levels/Shop.tscn");
    }

    private void OnPauseButtonPressed()
    {
        main.PauseGame();
        buttons.Visible = true;
        pauseButton.Visible = false;
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit(0);
    }
}