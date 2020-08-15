using Godot;

public class SettingMenue : Control
{
    private CheckBox fullScreenCheckBox;
    private CheckBox boderlessCheckBox;

    public override void _Ready()
    {
        fullScreenCheckBox = GetNode<CheckBox>("HBoxContainer/FullScreenCheckBox");
        boderlessCheckBox = GetNode<CheckBox>("HBoxContainer/BoderlessCheckBox");

        if (OS.WindowFullscreen)
            fullScreenCheckBox.Pressed = true;
        else
            fullScreenCheckBox.Pressed = false;

        if (OS.WindowBorderless)
            boderlessCheckBox.Pressed = true;
        else
            boderlessCheckBox.Pressed = false;
    }

    private void OnCheckBoxToggled(bool toggled)
    {
        if (toggled)
        {
            if (!OS.WindowFullscreen)
                OS.WindowFullscreen = true;
        }
        else
        {
            if (OS.WindowFullscreen)
                OS.WindowFullscreen = false;
        }
    }
    private void BoderlessCheckBoxToggled(bool toggled)
    {
        if (toggled)
        {
            if (!OS.WindowBorderless)
                OS.WindowBorderless = true;
        }
        else
        {
            if (OS.WindowBorderless)
                OS.WindowBorderless = false;
        }
    }
}
