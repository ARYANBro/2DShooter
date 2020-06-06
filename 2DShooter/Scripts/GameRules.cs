using Godot;
using System;

public class GameRules : Node2D
{
    /* Input */
    public override void _Input(InputEvent @event)
    {
        /* Quit */
        if (Input.IsActionJustPressed("ui_cancel"))
            GetTree().Quit();
    }
}