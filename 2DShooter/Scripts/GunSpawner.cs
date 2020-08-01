using Godot;
using System;

public class GunSpawner
{
    public void InitGun<T>(ref T weapon, PackedScene gunScene) where T : Gun, new()
    {
        weapon = new T();
        Shop.slots.Add(new Slot(weapon));
        Shop.guns.Add(weapon);

        weapon = gunScene.Instance() as T;
    }

    public void Spawn<T>(ref T gun, Vector2 position, float rotationDegrees, Node parent) where T : Gun
    {
        if (gun.IsUnlocked && !gun.AlreadySpawned)
        {
            gun.RotationDegrees = rotationDegrees;
            gun.Position = position;

            parent.AddChild(gun);

            gun.AlreadySpawned = true;
        }
    }

    public void GunUnlockCheck<T>(ref T gun, int index) where T : Gun
    {
        if (Shop.slots[index].Gun.ShopName == gun.ShopName && Shop.guns[index].IsUnlocked && Shop.guns[index].SetForSpawn)
        {
            gun.IsUnlocked = true;
            gun.SetForSpawn = true;
        }
    }
}