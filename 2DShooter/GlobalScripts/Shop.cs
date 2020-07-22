using Godot;
using System;


public class Shop : Node2D
{
    [Signal] public delegate void SRequestPause();

    [Export] public float slot0XP;
    [Export] public float slot1XP;
    [Export] public float slot2XP;

    private enum Slots
    {
        // Position
        Slot0 = 0, Slot1 = -250, Slot2 = -500
    };

    Slots currentSlot = Slots.Slot0;
    private Node2D guns;

    private TextureButton lockUnlockButton;
    private float currentXpCheck;

    public override void _Ready()
    {
        guns = GetNode<Node2D>("Slots");
        lockUnlockButton = GetNode<TextureButton>("LockUnlockButton");
        currentXpCheck = slot0XP;
    }

    public override void _Process(float delta)
    {
        // Position changes based on slot (Slot is the "X Position")
        MoveToSlot(currentSlot, delta, 6f);

        // Lock unlcock based on HighScore
        if (GameRules.HighScore < currentXpCheck)
            lockUnlockButton.Disabled = true;
        else
            lockUnlockButton.Disabled = false;
    }

    /* Just change the slot name
       For Right Arrow */

    // Change the xp check also
    private void OnRightSlideArrowPressed()
    {
        if (currentSlot == Slots.Slot0)
        {
            currentXpCheck = slot1XP;   
            currentSlot = Slots.Slot1;
        }
        else if (currentSlot == Slots.Slot1)
        {
            currentXpCheck = slot2XP;
            currentSlot = Slots.Slot2;
        }   
    }

    // For Left Arrow
    private void OnLeftSlideArrowPressed()
    {
        if (currentSlot == Slots.Slot2)
        {
            currentXpCheck = slot1XP;
            currentSlot = Slots.Slot1;
        }
        else if (currentSlot == Slots.Slot1)
        {
            currentXpCheck = slot0XP;
            currentSlot = Slots.Slot0;
        }
    }

    private void MoveToSlot(Slots slots, float delta, float accel)
    {
        guns.GlobalPosition = guns.GlobalPosition.LinearInterpolate(new Vector2((float)slots, 0), delta * accel);
    }

    private void OnGoBackButtonPressed()
    {
    }
}