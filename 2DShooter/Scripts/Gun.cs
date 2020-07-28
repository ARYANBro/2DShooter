using Godot;
using System;

public abstract class Gun : Node2D, IPickable
{
    public PackedScene weaponScene { get; set; }
    public Inventory inventory { get; set; }
    public bool isEquiped { get; set; } = false;
    public bool wantToEquipGun { get; set; }
    public virtual bool isUnlocked { get; set; } = false;
    public virtual bool AlreadySpawned { get; set; } = false;

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
    }

    public virtual void Equip()
    {
        inventory.AddItemToInventory(weaponScene);
        isEquiped = true;

        QueueFree();
    }

    public abstract void UnEquip();

    public bool ParentCheck => GetParent().GetType().Name == "Inventory";
}