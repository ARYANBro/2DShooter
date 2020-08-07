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

    InputHandler inputHandler = new InputHandler();

    public override void _Ready()
    {
        outLineParticles = GetNode<Particles2D>("UnEquipedParticles");
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;

        weaponScene = ResourceLoader.Load<PackedScene>("res://Assets/Weapons/Pistol.tscn");

        outLineParticles.Emitting = false;
    }


    public override void _Process(float delta)
    {
        if (inputHandler.UnEquipPressed(this))
            UnEquip();

        if (inputHandler.EquipedPressed(this))
            Equip();

        if (!ParentCheck)
            GetNode<GunComponent>("GunComponent").SetProcess(false);

        isEquiped = ParentCheck;
        
        Sprite GunSprite = GetNode<Sprite>("GunComponent/GunSprite");
        if (!ParentCheck)
        {
            GunSprite.Material.Set("shader_param/outline", true);
            outLineParticles.Emitting = true;
        }
        else
        {
            GunSprite.Material.Set("shader_param/outline", false);
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
}