using Godot;
using System;

public  class GameRules : Node
{
    public bool engineScaleCheck = false;

    public virtual void PauseGame()
    {
        GetTree().Paused = true;
        GetNode<TextureButton>("Hud/PauseMenue/PauseButton").Visible = false;
        GetTree().CurrentScene.GetNode<Control>("Hud/PauseMenue/PauseMenue").Visible = true;
    }

    public virtual void ResumeGame()
    {
        GetTree().Paused = false;
    }

    public void FixEngineScale()
    {
        Engine.TimeScale = 1.0f;
        engineScaleCheck = false;
    }

    public void EngineScaleCheck()
    {
        // Check if Engine scale is ok
        if (engineScaleCheck == true)
        {
            if (Engine.TimeScale < 1.0f)
                GetTree().CreateTimer(2f).Connect("timeout", this, "FixEngineScale");
        }
    }
}