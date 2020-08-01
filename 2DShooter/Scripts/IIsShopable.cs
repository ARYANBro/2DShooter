using Godot;
using System;

public interface IIsShopable
{
    string ShopName { get; set; }
    float XPCheck { get; set; }
    bool IsUnlocked { get; set; }
    bool SetForSpawn { get; set; }
    Vector2 SlotPosition { get; set; }
}