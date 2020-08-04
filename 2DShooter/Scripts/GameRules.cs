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

    async public void EngineScaleCheck()
    {
        // Check if Engine scale is ok
        if (engineScaleCheck == true)
        {
            if (Engine.TimeScale < 1.0f)
            {
                await ToSignal(GetTree().CreateTimer(2f), "timeout");
                Engine.TimeScale = 1.0f;
                engineScaleCheck = false;
            }
        }
    }
}