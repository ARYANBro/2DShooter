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
			Node pickable = Pickable.Instance();
			AddChild(pickable, true);

			((Node2D)pickable).Hide();
			GetTree().CreateTimer(0.02f).Connect("timeout", ((Node2D)pickable), "show");
		}
	}
	
	public void RemoveItemFromInventory(IPickable pickable)
	{
		foreach (Node child in GetChildren())
		{
			if (pickable.GetType().Name == child.GetType().Name)
				child.QueueFree();
		}
	}
}