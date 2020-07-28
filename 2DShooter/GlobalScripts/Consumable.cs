using Godot;
using System;

public abstract class Consumable : Area2D
{
    public abstract Player Player { get; set; }
}
