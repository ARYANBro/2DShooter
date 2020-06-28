using Godot;
using System;

public class Pistol : Node2D, IPickable
{
	[Export(PropertyHint.File, "Pistol.tscn")] public String path { get; set; }
	public Inventory inventory { get; set; }
	public bool isEquiped { get; set; }

	public override void _Ready()
	{
		inventory = (Inventory)GetTree().CurrentScene.FindNode("Inventory", true, false);
	}

	public void OnBodyEntered(object body)
	{
		if (body.GetType().Name == "Player" && !isEquiped)
			Equip();
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("UnEquip") && isEquiped)
			UnEquip();
		
		ParentCheck();
	}

	public void ParentCheck()
	{
		if (GetParent().GetType().Name == "Inventory")
			isEquiped = true;
		else
			isEquiped = false;
	}

	public void Equip()
	{
		inventory.AddItemToInventory(this);
		isEquiped = true;
	}

	public void UnEquip()
	{
		inventory.RemoveItemFromInventory(this);
	}
}
