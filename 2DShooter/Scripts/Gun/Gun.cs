using Godot;
using System;

public abstract class Gun : Node2D, IPickable, IIsShopable
{
    public PackedScene weaponScene { get; set; }
    public Inventory inventory { get; set; }
    public bool isEquiped { get; set; } = false;
    public bool wantToEquipGun { get; set; }
    public virtual bool AlreadySpawned { get; set; } = false;

    public abstract float XPCheck { get; set; }
    public abstract bool IsUnlocked { get; set; }
    public abstract string ShopName { get; set; }
    public abstract Vector2 SlotPosition { get; set; }
    public abstract bool SetForSpawn { get; set; }

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

    public bool ParentCheck => GetParent() is Inventory;
}