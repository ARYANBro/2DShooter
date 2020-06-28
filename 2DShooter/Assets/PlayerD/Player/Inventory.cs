using Godot;
using System;

public class Inventory : Node2D
{
	private PackedScene Pickable;

	public void AddItemToInventory(IPickable _pickable)
	{
		Pickable = GD.Load<PackedScene>(_pickable.path);
		if (Pickable != null && GetChildCount() == 0)
		{
			var pickable = Pickable.Instance();
			CallDeferred("add_child", pickable, true);
		}
	}

	public void RemoveItemFromInventory(IPickable pickable)
	{
		foreach (var child in GetChildren())
		{
			if (pickable.GetType().Name == child.GetType().Name)
				((Node)child).QueueFree();
		}
	}
}