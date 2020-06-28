using Godot;
using System;

public class RocketLauncher : Node2D, IPickable
{
	[Export(PropertyHint.File, "RocketLauncher.tscn")] public String path { get; set; }

	public Inventory inventory { get; set; }
	public bool isEquiped { get; set; }

	private PackedScene RocketLauncherScene;
	private Vector2 playerPos;

	public override void _Ready()
	{
		inventory = (Inventory)GetTree().CurrentScene.FindNode("Inventory", true, false);
		RocketLauncherScene = GD.Load<PackedScene>(path);
	}

	public void OnBodyEntered(object body)
	{
		if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0
		)
		{
			var player = body as Player;
			playerPos = player.GlobalPosition;
			Equip();
		}
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("UnEquip") && isEquiped)
			UnEquip();

		isEquiped = ParentCheck();
	}

	public bool ParentCheck()
	{
		return GetParent().GetType().Name == "Inventory";
	}

	public void Equip()
	{
		inventory.AddItemToInventory(this);
		isEquiped = true;
		QueueFree();
	}

	public void UnEquip()
	{
		inventory.RemoveItemFromInventory(this);
		RocketLauncher rocketLauncher = (RocketLauncher)RocketLauncherScene.Instance();
		rocketLauncher.GlobalPosition = playerPos;

		GetTree().CurrentScene.AddChild(rocketLauncher);
	}
}
