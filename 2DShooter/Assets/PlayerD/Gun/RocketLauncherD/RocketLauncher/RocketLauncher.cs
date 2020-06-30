using Godot;
using System;

public class RocketLauncher : Node2D, IPickable
{
    [Export(PropertyHint.File, "RocketLauncher.tscn")] public string path { get; set; }

    public Inventory inventory { get; set; }
    public bool isEquiped { get; set; }
    public bool wantToEquipGun { get; set; }
    [Export] public Texture outLineSprite;

    private PackedScene RocketLauncherScene;
    private Texture orignalTexture;

    public override void _Ready()
    {
        inventory = GetTree().CurrentScene.FindNode("Inventory", true, false) as Inventory;
        RocketLauncherScene = GD.Load<PackedScene>(path);
        orignalTexture = GetNode<Sprite>("GunComponent/GunSprite").Texture;
    }

    public void OnBodyEntered(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0)
            wantToEquipGun = true;
    }

    public void OnBodyExited(object body)
    {
        if (body.GetType().Name == "Player" && !isEquiped && inventory.GetChildCount() == 0)
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

    public bool ParentCheck => GetParent().GetType().Name == "Inventory";

    public void Equip()
    {
        inventory.AddItemToInventory(this);
        isEquiped = true;
        QueueFree();
    }

    public void UnEquip()
    {
        inventory.RemoveItemFromInventory(this);
        RocketLauncher rocketLauncher = RocketLauncherScene.Instance() as RocketLauncher;
        rocketLauncher.Position = GetTree().CurrentScene.GetNode<Player>("Player").Position;
        rocketLauncher.RotationDegrees = Utlities.LookAtMouse(GetGlobalMousePosition(), rocketLauncher.Position);
        GetTree().CurrentScene.AddChild(rocketLauncher);
    }
}