using Godot;
using System;

public class Shop : Node2D
{
    [Export] public float slot0XP;
    [Export] public string slot0Name;
    [Export] public float slot1XP;
    [Export] public string slot1Name;
    [Export] public float slot2XP;
    [Export] public string slot2Name;

    public struct Slot
    {
        public enum SlotsPosition
        {
            Slot0 = 0, Slot1 = -250, Slot2 = -500
        };

        public static bool isUnlocked;

        public static string gunName;
        public static SlotsPosition slotPosition;
    };

    private Node2D guns;
    public Slot currentSlot;
    public float currentXpCheck;
    private TextureButton lockUnlockButton;
    
    public override void _Ready()
    {
        guns = GetNode<Node2D>("Slots");
        lockUnlockButton = GetNode<TextureButton>("LockUnlockButton");

        Slot.isUnlocked = false;
        Slot.slotPosition = Slot.SlotsPosition.Slot0;
        Slot.gunName = slot0Name;
        GameRules.ResumeGame();
    }

    public override void _Process(float delta)
    {
        // Position changes based on slot Positoin
        MoveToSlot(Slot.slotPosition, delta, 6f);

        //Gun is unlocked ?
        if (GameRules.HighScore >= currentXpCheck)
            Slot.isUnlocked = true;
        else
            Slot.isUnlocked = false;

        // State of lockUnlock Button
        if (Slot.isUnlocked)
            lockUnlockButton.Disabled = false;
        else
            lockUnlockButton.Disabled = true;

        if (Slot.slotPosition == Slot.SlotsPosition.Slot0)
            Slot.gunName = slot0Name;
        else if (Slot.slotPosition == Slot.SlotsPosition.Slot1)
            Slot.gunName = slot1Name;
        else if (Slot.slotPosition == Slot.SlotsPosition.Slot2)
            Slot.gunName = slot2Name;
        else
            Slot.gunName = "Error";

    }

    /*  Just change the slot name
        For Right Arrow, change the xp check also */
    private void OnRightSlideArrowPressed()
    {
        if (Slot.slotPosition == Slot.SlotsPosition.Slot0)
        {
            currentXpCheck = slot1XP;
            Slot.slotPosition = Slot.SlotsPosition.Slot1;
        }
        else if (Slot.slotPosition == Slot.SlotsPosition.Slot1)
        {
            currentXpCheck = slot2XP;
            Slot.slotPosition = Slot.SlotsPosition.Slot2;
        }
    }

    // For Left Arrow
    private void OnLeftSlideArrowPressed()
    {
        if (Slot.slotPosition == Slot.SlotsPosition.Slot2)
        {
            currentXpCheck = slot1XP;
            Slot.slotPosition = Slot.SlotsPosition.Slot1;
        }
        else if (Slot.slotPosition == Slot.SlotsPosition.Slot1)
        {
            currentXpCheck = slot0XP;
            Slot.slotPosition = Slot.SlotsPosition.Slot0;
        }
    }

    private void MoveToSlot(Slot.SlotsPosition slots, float delta, float accel)
    {
        guns.GlobalPosition = guns.GlobalPosition.LinearInterpolate(new Vector2((float)slots, 0), delta * accel);
    }

    private void OnGoBackButtonPressed()
    {
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}