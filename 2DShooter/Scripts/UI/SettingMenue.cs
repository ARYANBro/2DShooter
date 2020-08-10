using Godot;
using System;

public class SettingMenue : Control
{
    private void OnCheckBoxToggled(bool checkBoxChecked)
    {
        if (checkBoxChecked)
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
}
