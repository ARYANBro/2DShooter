using Godot;
using System;

public class GunSpawner
{
    public IPickable InitGun(PackedScene gunScene)
    {
        return gunScene.Instance() as IPickable;
    }

    public void Spawn<T>(ref T gun, Vector2 position, float rotationDegrees, Node parent) where T : Node2D, IPickable
    {
        if (gun.isUnlocked && !gun.AlreadySpawned)
        {
            gun.RotationDegrees = rotationDegrees;
            gun.Position = position;

            parent.AddChild(gun);

            gun.AlreadySpawned = true;
        }
    }

    public void GunUnlockCheck<T>(ref T _gun, Shop.Slot.GunNames checkName) where T : IPickable
    {
        var gun = _gun as IPickable;

        if (Shop.currentSlot.gunName == checkName && Shop.currentSlot.isUnlocked)
            gun.isUnlocked = true;
    }
}