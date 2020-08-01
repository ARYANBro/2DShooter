using Godot;
using System;

public class Shotgun : Gun
{
    [Export] public Texture outLineSprite;
    public override float XPCheck { get; set; } = 50f;
    public override bool IsUnlocked { get; set; } = false;
    public override Vector2 SlotPosition { get; set; } = new Vector2(-250f, 0f);
    public override string ShopName { get; set; } = "Shotgun";
    public override bool SetForSpawn { get; set; } = false;

    private Texture orignalTexture;

    public override void _EnterTree()
    {
		AlreadySpawned = false;
    }

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        orignalTexture = GetNode<Sprite>("GunComponent/GunSprite").Texture;

		weaponScene = ResourceLoader.Load<PackedScene>("Assets/Weapons/Shotgun.tscn");
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

        SetForSpawn = Shop.slots[1].Gun.SetForSpawn;
    }

    public override void UnEquip()
    {
        inventory.RemoveItemFromInventory(this);
        var shotgun = weaponScene.Instance() as Shotgun;

        shotgun.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;
        shotgun.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), shotgun.Position);

        GetTree().CurrentScene.AddChild(shotgun);
    }
}