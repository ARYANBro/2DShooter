using Godot;
using System;

public class Inventory : Node2D
{
	[Export] public int maxItemInInventory;

	public void AddItem(IInInventory inInventory)
	{
		inInventory.Equip();
	}

	public void RemoveItem(IInInventory inInventory)
	{
		inInventory.UnEquip();
	}
}
