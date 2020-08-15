using Godot;

public class RocketLauncher : Gun
{
    public Particles2D outLineParticles;

    public override float XPCheck { get; set; } = 1000f;
    public override Vector2 SlotPosition { get; set; } = new Vector2(-500f, 0f);
    public override string ShopName { get; set; } = "RocketLauncher";
    public override bool IsUnlocked { get; set; } = false;
    public override bool SetForSpawn { get; set; } = false;

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        outLineParticles = GetNode<Particles2D>("UnEquipedParticles");
        weaponScene = ResourceLoader.Load<PackedScene>("res://Assets/Weapons/RocketLauncher.tscn");
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
        if (Input.IsActionJustPressed("UnEquip") && isEquiped && !wantToEquipGun)
            UnEquip();

        if (Input.IsActionJustPressed("Equip") && !isEquiped && wantToEquipGun)
            Equip();

        if (!ParentCheck)
            GetNode<GunComponent>("GunComponent").SetProcess(false);

        isEquiped = ParentCheck;
        if (!ParentCheck)
        {
            // Add outline when not equiped
            outLineParticles.Emitting = true;
            GetNode<Sprite>("GunComponent/GunSprite").Material.Set("shader_param/outline", true);
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
        var rocketLauncher = weaponScene.Instance() as RocketLauncher;

        rocketLauncher.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;
        rocketLauncher.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), rocketLauncher.Position);

        GetTree().CurrentScene.AddChild(rocketLauncher);
    }
}
