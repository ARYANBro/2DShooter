using Godot;
using System;

public class Shotgun : Gun, IPickable
{
	[Export(PropertyHint.File, "Shotgun.tscn")] public string path { get; set; }
	public bool isUnlocked { get; set; }
	[Export] public Texture outLineSprite;
	private PackedScene ShotgunScene;
	private Texture orignalTexture;

	public override void _Ready()
	{
		inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
		orignalTexture = GetNode<Sprite>("GunComponent/GunSprite").Texture;
		ShotgunScene = GD.Load<PackedScene>(path);
		isUnlocked = false;
	}

	public void OnBodyEntered(object body)
	{
		if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0 &&
			body.GetType().Name != "Pistol" && body.GetType().Name != "RocketLauncher")
			wantToEquipGun = true;
	}

	public void OnBodyExited(object body)
	{
		if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0 && 
			body.GetType().Name != "Pistol" && body.GetType().Name != "RocketLauncher")
			wantToEquipGun = false;
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("UnEquip") && isEquiped)
			UnEquip();

		if (wantToEquipGun && Input.IsActionJustPressed("Equip"))
			Equip();

		if (!ParentCheck)
			GetNode<GunComponent>("GunComponent").SetProcess(false);

		isEquiped = ParentCheck;
		// Add outline when not equiped
		if (!ParentCheck)
		{
			if (outLineSprite != null)
				GetNode<Sprite>("GunComponent/GunSprite").Texture = outLineSprite;
		}
		else
			GetNode<Sprite>("GunComponent/GunSprite").Texture = orignalTexture;

		// Unlock shotgun

	}

	public override void UnEquip()
	{
		inventory.RemoveItemFromInventory(this);
		var shotgun = ShotgunScene.Instance() as Shotgun;

		shotgun.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;
		shotgun.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), shotgun.Position);

		GetTree().CurrentScene.AddChild(shotgun);
	}
}