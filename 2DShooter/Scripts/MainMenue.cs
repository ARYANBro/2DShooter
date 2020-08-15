using Godot;

public class MainMenue : Node2D
{
    private Control buttons;
    private Control goBackButton;
    private Control settingsMenue;

    private bool SettingsMenueVisible
    { 
        get
        {
            return settingsMenue.Visible && goBackButton.Visible;
        }

        set
        {
            settingsMenue.Visible = value;
            goBackButton.Visible = value;
        } 
    }

    public override void _Ready()
    {
        settingsMenue = GetTree().CurrentScene.GetNode<Control>("MainMenueHud/SettingMenue");
        goBackButton = GetTree().CurrentScene.GetNode<Control>("MainMenueHud/GobackButton");
        buttons = GetTree().CurrentScene.GetNode<Control>("MainMenueHud/Buttons");

        if (SettingsMenueVisible)
            OnGoBackButtonPressed();
    }

    private void PlayButtonPressed()
    {
        GetTree().ChangeScene("res://Levels/Main.tscn");
    }

    private void ExitButtonPressed()
    {
        GetTree().Quit(0);
    }

    private void OnSettingsButtonPressed()
    {
        SettingsMenueVisible = true;
        buttons.Visible = false;
    }

    private void OnGoBackButtonPressed()
    {
        SettingsMenueVisible = false;
        buttons.Visible = true;
    }
}