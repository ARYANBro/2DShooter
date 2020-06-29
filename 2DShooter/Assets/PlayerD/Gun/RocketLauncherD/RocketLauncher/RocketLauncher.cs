using Godot;
using System;

public class RocketLauncher : Node2D, IPickable
{
    [Export(PropertyHint.File, "RocketLauncher.tscn")] public string path { get; set; }

    public Inventory inventory { get; set; }
    public bool isEquiped { get; set; }
    public bool wantToEquipGun { get; set; }

    private PackedScene RocketLauncherScene;

    public override void _Ready()
    {
        inventory = (Inventory)GetTree().CurrentScene.FindNode("Inventory", true, false);
        RocketLauncherScene = GD.Load<PackedScene>(path);
    }

    public void OnBodyEntered(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0)
            wantToEquipGun = true;
    }

    public void OnBodyExited(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0)
            wantToEquipGun = false;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("UnEquip") && isEquiped)
            UnEquip();
        if (!ParentCheck)
            GetNode<GunComponent>("GunComponent").SetProcess(false);
        if (wantToEquipGun && Input.IsActionJustPressed("Equip"))
            Equip();

        isEquiped = ParentCheck;
    }

    public bool ParentCheck => GetParent().GetType().Name == "Inventory";

    public void Equip()
    {
        inventory.AddItemToInventory(this);
        isEquiped = true;
        QueueFree();
    }

    public void UnEquip()
    {
        inventory.RemoveItemFromInventory(this);
        RocketLauncher rocketLauncher = RocketLauncherScene.Instance() as RocketLauncher;
        rocketLauncher.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;
        rocketLauncher.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), rocketLauncher.Position);
        GetTree().CurrentScene.AddChild(rocketLauncher);
    }
}