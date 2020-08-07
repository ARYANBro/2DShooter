using Godot;
using System;

public abstract class Consumable : Area2D
{
    public virtual Player Player { get; set; }

    public override void _Ready()
    {
        Player = GetTree().CurrentScene.GetNode<Player>("Player");
    }
}