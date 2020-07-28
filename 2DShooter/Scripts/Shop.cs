using Godot;
using System;
using System.Collections.Generic;

public class Shop : Node2D
{
    [Export] public float slot0XP;
    [Export] public float slot1XP;
    [Export] public float slot2XP;
    [Export(PropertyHint.Flags, "Slot.GunNames")] public Slot.GunNames slot0GunName;
    [Export(PropertyHint.Flags, "Slot.GunNames")] public Slot.GunNames slot1GunName;
    [Export(PropertyHint.Flags, "Slot.GunNames")] public Slot.GunNames slot2GunName;
    public Label highScoreCounter;
    
    public struct Slot
    {
        public enum GunNames
        {
            Pistol, Shotgun, RocketLauncher
        };

        public enum SlotsPosition
        {
            Slot0 = 0, Slot1 = -250, Slot2 = -500
        };

        public bool isUnlocked;

        public GunNames gunName;
        public SlotsPosition slotPosition;
    };

    private Node2D guns;
    public static Slot currentSlot;
    public float currentXpCheck;
    private TextureButton lockUnlockButton;

    public override void _Ready()
    {
        guns = GetNode<Node2D>("Slots");
        lockUnlockButton = GetNode<TextureButton>("LockUnlockButton");
        highScoreCounter = GetNode<Label>("HighScore/Label");

        currentSlot.isUnlocked = false;
        currentSlot.slotPosition = Slot.SlotsPosition.Slot0;

        GameRules.ResumeGame();
    }

    public override void _Process(float delta)
    {
        if (currentSlot.slotPosition == Slot.SlotsPosition.Slot0)
            currentSlot.gunName = slot0GunName;
        else if (currentSlot.slotPosition == Slot.SlotsPosition.Slot1)
            currentSlot.gunName = slot1GunName;
        else if (currentSlot.slotPosition == Slot.SlotsPosition.Slot2)
            currentSlot.gunName = slot2GunName;

        // Position changes based on slot Positoin
        MoveToSlot(currentSlot.slotPosition, delta, 6f);

        //Gun is unlocked ?
        if (GameRules.HighScore >= currentXpCheck)
            currentSlot.isUnlocked = true;
        else
            currentSlot.isUnlocked = false;

        // State of lockUnlock Button
        if (currentSlot.isUnlocked)
            lockUnlockButton.Disabled = false;
        else
            lockUnlockButton.Disabled = true;

        // Set the higScore
        highScoreCounter.Text = GameRules.HighScore.ToString();
    }

    /*  Just change the slot name
        For Right Arrow, change the xp check also */
    private void OnRightSlideArrowPressed()
    {
        if (currentSlot.slotPosition == Slot.SlotsPosition.Slot0)
        {
            currentXpCheck = slot1XP;
            currentSlot.slotPosition = Slot.SlotsPosition.Slot1;
        }
        else if (currentSlot.slotPosition == Slot.SlotsPosition.Slot1)
        {
            currentXpCheck = slot2XP;
            currentSlot.slotPosition = Slot.SlotsPosition.Slot2;
        }
    }

    // For Left Arrow
    private void OnLeftSlideArrowPressed()
    {
        if (currentSlot.slotPosition == Slot.SlotsPosition.Slot2)
        {
            currentXpCheck = slot1XP;
            currentSlot.slotPosition = Slot.SlotsPosition.Slot1;
        }
        else if (currentSlot.slotPosition == Slot.SlotsPosition.Slot1)
        {
            currentXpCheck = slot0XP;
            currentSlot.slotPosition = Slot.SlotsPosition.Slot0;
        }
    }

    private void MoveToSlot(Slot.SlotsPosition slots, float delta, float accel)
    {
        guns.GlobalPosition = guns.GlobalPosition.LinearInterpolate(new Vector2((float)slots, 0), delta * accel);
    }

    private void OnGoBackButtonPressed()
    {
        GetTree().ChangeScene("res://Levels/Main.tscn");
    }
}