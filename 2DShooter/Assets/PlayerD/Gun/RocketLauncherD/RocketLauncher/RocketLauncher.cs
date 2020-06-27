using Godot;
using System;
using System.ComponentModel.Design;

public class RocketLauncher : Node2D, IInInventory
{
	public Area2D equipArea { get; set; }
	public bool IsEquiped { get; set; }
	[Export] public NodePath inventoryPath;

	public override void _Ready()
	{
		equipArea = GetNode<Area2D>("EquipArea");
	}

	public void Equip()
	{
		var inventory = GetTree().CurrentScene.FindNode("Inventory", true, false);
		if (inventory != null)
			inventory.AddChild(this);
	}

	public void UnEquip()
	{
		var inventory = GetTree().CurrentScene.FindNode("Inventory", true, false);
		if (inventory != null)
			GD.Print("UnEquipped");
	}

	public override void _Process(float delta)
	{
		if (!IsAParentOf(GetTree().CurrentScene.FindNode("Inventory", true, false)))
			IsEquiped = false;
		else if (IsAParentOf(GetTree().CurrentScene.FindNode("Inventory", true, false)))
			IsEquiped = true;

		if (Input.IsActionJustPressed("UnEquip") && IsEquiped)
		{
			UnEquip();
			IsEquiped = false;
		}
	}

	private void OnEquipAreaBodyEntered(object body)
	{
		if (body.GetType().Name == "Player" && !IsEquiped)
		{
			GD.Print(!IsEquiped);
			Equip();
		}
	} 
}
