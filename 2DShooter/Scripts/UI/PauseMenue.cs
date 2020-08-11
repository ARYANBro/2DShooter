using Godot;
using System;

public class PauseMenue : Control
{
    private MainRoot main;
    private HBoxContainer buttons;
    private TextureButton pauseButton;
    private Control settingMenue;

    public override void _Ready()
    {
        main = GetTree().CurrentScene as MainRoot;
        pauseButton = GetNode<TextureButton>("PauseButton");
        buttons = GetNode<HBoxContainer>("Buttons");
        settingMenue = GetTree().CurrentScene.GetNode<Control>("Hud/SettingMenue");
     
        buttons.Hide();

    }

    private void OnResumeButtonPressed()
    {
        main.ResumeGame();
        
        buttons.Visible = false;
        pauseButton.Visible = true; 

        if (settingMenue.Visible)
            settingMenue.Visible = false;
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

    private void OnSettingMenueButtonPressed()
    {
        if (settingMenue.Visible)
            settingMenue.Visible = false;
        else
            settingMenue.Visible = true;
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}