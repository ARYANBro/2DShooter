using Godot;
using System;

public class Shotgun : Gun, IPickable
{
	[Export(PropertyHint.File, "Shotgun.tscn")] public string path { get; set; }
	[Export] public Texture outLineSprite;
	public static bool isUnlocked = true;
	
	private PackedScene ShotgunScene;
	private Texture orignalTexture;

	public override void _Ready()
	{
		inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
		orignalTexture = GetNode<Sprite>("GunComponent/GunSprite").Texture;
		ShotgunScene = GD.Load<PackedScene>(path);
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
		if (!ParentCheck)
			GetNode<GunComponent>("GunComponent").SetProcess(false);
		if (wantToEquipGun && Input.IsActionJustPressed("Equip"))
			Equip();

		isEquiped = ParentCheck;
		if (!ParentCheck)
		{
			if (outLineSprite != null)
				GetNode<Sprite>("GunComponent/GunSprite").Texture = outLineSprite;
		}
		else
			GetNode<Sprite>("GunComponent/GunSprite").Texture = orignalTexture;
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