using System;
using Godot;

public class Healthbar : TextureProgress
{
    private Player player;

    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<Player>("Player");
    }

    public override void _Process(float delta)
    {
        Value = player.hp;
    }
}
