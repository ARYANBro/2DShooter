using Godot;
using System;

public class Gun : Node2D
{
    public Inventory inventory { get; set; }
    public bool isEquiped { get; set; }
    public bool wantToEquipGun { get; set; }
    string path { get; set; }
    public PackedScene gunScene;

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        gunScene = GD.Load<PackedScene>(path);
    }

    public void Equip()
    {
        inventory.AddItemToInventory((IPickable)this);
        isEquiped = true;
        QueueFree();
    }

    public virtual void UnEquip()
    {
        /*     Unequip
          Called from child */
    }

    public bool ParentCheck => GetParent().GetType().Name == "Inventory";
}