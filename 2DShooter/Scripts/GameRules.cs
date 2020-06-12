using System;
using Godot;

public class GameRules : Node2D
{
    [Signal] public delegate void OnPlayerDied();

    public override void _Ready()
    {
        GetNode<Player>("Player").Connect("PlayerDiedHandler", this, "PlayerDied");
    }

    private void PlayerDied()
    {
        EmitSignal("OnPlayerDied");
        GetTree().Paused = true;
    }
}