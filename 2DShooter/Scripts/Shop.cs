using Godot;
using System;
using System.Collections.Generic;
public class Shop : GameRules
{
    public Button spawnButton;
    public float currentXpCheck;
    public static Slot currentSlot = new Slot();
    public static List<Gun> guns = new List<Gun>();
    public static List<Slot> slots = new List<Slot>();

    Node2D SlotsNode;
    Label highScoreCounter;
    TextureButton lockUnlockButton;

    public override void _Ready()
    {
        SlotsNode = GetNode<Node2D>("Slots");
        spawnButton = GetNode<Button>("SpawnButton");
        highScoreCounter = GetNode<Label>("HighScore/Label");
        lockUnlockButton = GetNode<TextureButton>("LockUnlockButton");

        currentSlot = slots[0];

        if (GetTree().Paused)
            GetTree().Paused = false;
       
    }

    public override void _Process(float delta)
    {
        SlotProcess(delta);
        highScoreCounter.Text = MainRoot.HighScore.ToString();

        if (currentSlot.Gun.IsUnlocked)
        {
            spawnButton.Show();
            spawnButton.SetProcess(true);
        }
        else {
            spawnButton.Hide();
            spawnButton.SetProcess(false);
        }
    }

    void OnRightSlideArrowPressed()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (currentSlot == slots[i])
            {
                if (currentSlot != slots[slots.Count - 1])
                {
                    currentSlot = slots[i + 1];
                    return;
                }
                else return;
            }
        }
    }

    // For Left Arrow
    void OnLeftSlideArrowPressed()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (currentSlot == slots[i])
            {
                if (currentSlot != slots[0])
                {
                    currentSlot = slots[i - 1];
                    return;
                }
                else return;
            }
        }
    }

    void SlotProcess(float delta)
    {
        MoveToSlot(currentSlot.Gun.SlotPosition, delta * 6f);
        if (currentSlot.Gun.SetForSpawn)
            spawnButton.Disabled = true;
        else
            spawnButton.Disabled = false;

        foreach (var gun in guns)
        {
            if (MainRoot.HighScore >= gun.XPCheck)
            {
                gun.IsUnlocked = true;
            }
            else {
                gun.IsUnlocked = false;
            }
        }

        foreach (var slot in slots)
        {
            if (MainRoot.HighScore >= slot.Gun.XPCheck)
            {
                slot.Gun.IsUnlocked = true;
            }
            else {
                slot.Gun.IsUnlocked = false;
            }
        }

        // State of lockUnlock Button
        if (currentSlot.Gun.IsUnlocked)
            lockUnlockButton.Disabled = false;
        else
            lockUnlockButton.Disabled = true;
    }

    void MoveToSlot(Vector2 slotPosition, float delta)
    {
        SlotsNode.GlobalPosition = SlotsNode.GlobalPosition.LinearInterpolate(slotPosition, delta);
    }

    void OnGoBackButtonPressed()
    {
        GetTree().ChangeScene("res://Levels/Main.tscn");
    }

    void OnSpawnButtonPressed()
    {

        for (int i = 0; i < slots.Count; i++)
        {
            if (currentSlot == slots[i])
            {
                guns[i].SetForSpawn = true;
                return;
            }
        }
    }
}