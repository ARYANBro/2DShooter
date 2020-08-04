using Godot;
using System;

public class Pistol : Gun
{
    public Particles2D outLineParticles;

    public override float XPCheck { get; set; } = 0f;
    public override bool IsUnlocked { get; set; } = true;
    public override bool SetForSpawn { get; set; } = true;
    public override string ShopName { get; set; } = "Pistol";
    public override Vector2 SlotPosition { get; set; } = new Vector2(0f, 0f);

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        outLineParticles = GetNode<Particles2D>("UnEquipedParticles");

        weaponScene = ResourceLoader.Load<PackedScene>("res://Assets/Weapons/Pistol.tscn");

        outLineParticles.Emitting = false;
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
            GetNode<Sprite>("GunComponent/GunSprite").Material.Set("shader_param/outline", true);
            outLineParticles.Emitting = true;
        }
        else
        {
            GetNode<Sprite>("GunComponent/GunSprite").Material.Set("shader_param/outline", false);
            outLineParticles.Emitting = false;
        }
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