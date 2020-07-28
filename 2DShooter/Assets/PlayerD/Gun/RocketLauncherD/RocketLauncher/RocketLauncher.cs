using Godot;
using System;

public class RocketLauncher : Gun, IPickable
{
    [Export(PropertyHint.File, "RocketLauncher.tscn")] public string path { get; set; }
    [Export] public Texture outLineSprite;
    public bool isUnlocked { get; set; } = false;
    public bool AlreadySpawned { get; set; } = false;

    private PackedScene RocketLauncherScene;
    private Texture orignalTexture;

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        orignalTexture = GetNode<Sprite>("GunComponent/GunSprite").Texture;

        RocketLauncherScene = GD.Load<PackedScene>(path);
    }

    public void OnBodyEntered(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0 &&
            body.GetType().Name != "Pistol" && body.GetType().Name != "Shotgun")
            wantToEquipGun = true;

    }

    public void OnBodyExited(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0 &&
            body.GetType().Name != "Pistol" && body.GetType().Name != "Shotgun")
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
    }

    public override void UnEquip()
    {
        inventory.RemoveItemFromInventory(this);
        var rocketLauncher = RocketLauncherScene.Instance() as RocketLauncher;

        rocketLauncher.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;
        rocketLauncher.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), rocketLauncher.Position);

        GetTree().CurrentScene.AddChild(rocketLauncher);
    }
}
