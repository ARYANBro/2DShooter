using Godot;
using System.Collections.Generic;
public class Shop : GameRules
{
    public Button spawnButton;
    public float currentXpCheck;
    public static Slot currentSlot = new Slot();
    public static List<Gun> guns = new List<Gun>();
    public static List<Slot> slots = new List<Slot>();

    private Node2D SlotsNode;
    private Label highScoreCounter;
    private Label lockUnlockButton;

    public override void _Ready()
    {
        SlotsNode = GetNode<Node2D>("Slots");
        spawnButton = GetNode<Button>("SpawnButton");
        highScoreCounter = GetNode<Label>("HighScore/Label");
        lockUnlockButton = GetNode<Label>("LockUnlockButton");

        UpdateHighScore();

        currentSlot = slots[0];

        if (GetTree().Paused)
            ResumeGame();
    }

    public override void _Process(float delta)
    {
        SlotProcess(delta);
        LockUnlockButtonCheck();
        SpawnButtonUnlockedCheck();
    }

    public override void PauseGame()
    {
        GetTree().Paused = true;
    }

    private void UpdateHighScore()
    {
        highScoreCounter.Text = MainRoot.HighScore.ToString();
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
        GunUnlockCheck();
    }

    private void LockUnlockButtonCheck()
    {
        // State of lockUnlock Button
        if (currentSlot.Gun.IsUnlocked)
        {
            lockUnlockButton.Text = "UNLOCKED";
            lockUnlockButton.RectPosition = new Vector2(0f, 135f);
        }
        else
        {
            lockUnlockButton.Text = $"{ currentSlot.Gun.XPCheck } POINTS NEEDED";
            lockUnlockButton.RectPosition = new Vector2(10f, 135f);
        }
    }

    private void SpawnButtonUnlockedCheck()
    {
        if (currentSlot.Gun.SetForSpawn)
            spawnButton.Disabled = true;
        else
            spawnButton.Disabled = false;

        if (currentSlot.Gun.IsUnlocked)
        {
            spawnButton.Show();
            spawnButton.SetProcess(true);
        }
        else
        {
            spawnButton.Hide();
            spawnButton.SetProcess(false);
        }
    }

    private void GunUnlockCheck()
    {
        foreach (var gun in guns)
        {
            if (MainRoot.HighScore >= gun.XPCheck)
                gun.IsUnlocked = true;
            else
                gun.IsUnlocked = false;
        }

        foreach (var slot in slots)
        {
            if (MainRoot.HighScore >= slot.Gun.XPCheck)
                slot.Gun.IsUnlocked = true;
            else
                slot.Gun.IsUnlocked = false;
        }
    }

    private void MoveToSlot(Vector2 slotPosition, float delta)
    {
        SlotsNode.GlobalPosition = SlotsNode.GlobalPosition.LinearInterpolate(slotPosition, delta);
    }

    private void OnGoBackButtonPressed()
    {
        GetTree().ChangeScene("res://Levels/Main.tscn");
    }

    private void OnSpawnButtonPressed()
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