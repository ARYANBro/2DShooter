using Godot;
using System;

public class Shotgun : Gun
{
    public override float XPCheck { get; set; } = 500f;
    public override bool IsUnlocked { get; set; } = false;
    public override bool SetForSpawn { get; set; } = false;
    public override string ShopName { get; set; } = "Shotgun";
    public override Vector2 SlotPosition { get; set; } = new Vector2(-250f, 0f);

    public Particles2D outLineParticles;

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        outLineParticles = GetNode<Particles2D>("UnEquipedParticles");

        weaponScene = ResourceLoader.Load<PackedScene>("Assets/Weapons/Shotgun.tscn");
    }

    public void OnBodyEntered(object body)
    {
        if (body is Player && !isEquiped && inventory.GetChildCount() == 0 && !(body is Gun))
            wantToEquipGun = true;
    }

    public void OnBodyExited(object body)
    {
        if (body is Player && !isEquiped && inventory.GetChildCount() == 0 && !(body is Gun))
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
        var shotgun = weaponScene.Instance() as Shotgun;

        shotgun.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;
        shotgun.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), shotgun.Position);

        GetTree().CurrentScene.AddChild(shotgun);
    }
}