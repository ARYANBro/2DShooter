using Godot;
using System;

public interface IInInventory
{
    Area2D equipArea { get; set; }
    bool IsEquiped { get; set; }

    void Equip();
    void UnEquip();
}