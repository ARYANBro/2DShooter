using Godot;
using System;
using System.Collections.Generic;
public class Shop : Node2D
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

        GameRules.ResumeGame();
    }

    public override void _Process(float delta)
    {
        SlotProcess(delta);
        highScoreCounter.Text = GameRules.HighScore.ToString();

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

    private void OnRightSlideArrowPressed()
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
    private void OnLeftSlideArrowPressed()
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

    private void SlotProcess(float delta)
    {
        MoveToSlot(currentSlot.Gun.SlotPosition, delta * 6f);

        foreach (var slot in slots)
        {
            if (GameRules.HighScore >= slot.Gun.XPCheck)
                slot.Gun.IsUnlocked = true;

            else
                slot.Gun.IsUnlocked = false;
        }

        // State of lockUnlock Button
        if (currentSlot.Gun.IsUnlocked)
            lockUnlockButton.Disabled = false;
        else
            lockUnlockButton.Disabled = true;
    }

    private void MoveToSlot(Vector2 slotPosition, float delta)
    {
        SlotsNode.GlobalPosition = SlotsNode.GlobalPosition.LinearInterpolate(slotPosition, delta);
    }

    private void OnGoBackButtonPressed()
    {
        slots.Clear();
        GetTree().ChangeScene("res://Levels/Main.tscn");
    }

    void OnSpawnButtonPressed()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (currentSlot == slots[i])
            {
                guns[i].SetForSpawn = true;
                spawnButton.Hide();
                return;
            }
        }
    }
}