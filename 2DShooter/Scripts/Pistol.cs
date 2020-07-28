using Godot;
using System;

public class Pistol : Gun
{
    [Export] public Texture outLineSprite;
    private Texture orignalTexture;
    public override bool isUnlocked { get; set; } = true;

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
            if (outLineSprite != null) GetNode<Sprite>("GunComponent/GunSprite").Texture = outLineSprite;
        }
        else
            GetNode<Sprite>("GunComponent/GunSprite").Texture = orignalTexture;
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