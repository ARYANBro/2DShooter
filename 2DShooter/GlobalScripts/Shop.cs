using Godot;
using System;

public class Shop : Node2D
{
    public AnimationPlayer gunAnimPlayer;

    private enum Slots
    {
        Slot0 = 0, Slot1 = -250, Slot2 = -500
    };

    Slots currentSlot;
    private Node2D guns;

    public override void _Ready()
    {
        guns = GetNode<Node2D>("Slots");
        currentSlot = Slots.Slot0;
    }

    public override void _Process(float delta)
    {
        MoveToSlot(currentSlot, delta, 6f);
    }

    private void OnRightSlideArrowPressed()
    {
        if (currentSlot == Slots.Slot0)
            currentSlot = Slots.Slot1;
        else if (currentSlot == Slots.Slot1)
            currentSlot = Slots.Slot2;

    }

    private void OnLeftSlideArrowPressed()
    {
        if (currentSlot == Slots.Slot2)
            currentSlot = Slots.Slot1;
        else if (currentSlot == Slots.Slot1)
            currentSlot = Slots.Slot0;
    }

    private void MoveToSlot(Slots slots, float delta, float accel)
    {
        guns.GlobalPosition = guns.GlobalPosition.LinearInterpolate(new Vector2((float)slots, 0), delta * accel);
    }
}