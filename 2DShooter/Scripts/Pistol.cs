using Godot;
using System;

public class Pistol : Gun
{
    [Export] public Texture outLineSprite;
    public override Vector2 SlotPosition { get; set; } = new Vector2(0f, 0f);
    public override float XPCheck { get; set; } = 0f;
    private Texture orignalTexture;
    public override bool IsUnlocked { get; set; } = true;
    public override string ShopName { get; set; } = "Pistol";
    public override bool SetForSpawn { get; set; } = true;

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        orignalTexture = GetNode<Sprite>("GunComponent/GunSprite").Texture;

        weaponScene = ResourceLoader.Load<PackedScene>("res://Assets/Weapons/Pistol.tscn");
    }

    public void OnBodyEntered(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0 &&
        body.GetType().Name != "RocketLauncher" && body.GetType().Name != "Shotgun")
            wantToEquipGun = true;
    }

    public void OnBodyExited(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0 &&
         body.GetType().Name != "RocketLauncher" && body.GetType().Name != "Shotgun")
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
        if (!ParentCheck)
        {
            if (outLineSprite != null)
                GetNode<Sprite>("GunComponent/GunSprite").Texture = outLineSprite;
        }
        else
            GetNode<Sprite>("GunComponent/GunSprite").Texture = orignalTexture;

        SetForSpawn = Shop.slots[0].Gun.SetForSpawn;
    }
    public override void UnEquip()
    {
        inventory.RemoveItemFromInventory(this);
        Pistol pistol = weaponScene.Instance() as Pistol;

        pistol.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;

        pistol.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), pistol.Position);
        GetTree().CurrentScene.AddChild(pistol);
    }
}