using Godot;
using System;

public class GunSpawner
{
    private Node2D gunRoot;

    public Node2D InitializeGun(PackedScene gunScene)
    {
        return gunRoot = gunScene.Instance() as Node2D;
    }

    public void Spawn(IPickable gun, Vector2 position, float rotationDegrees, Node2D parent)
    {
        if (gun.isUnlocked)
        {
            gunRoot.RotationDegrees = rotationDegrees;
            gunRoot.Position = position;
            parent.AddChild(gunRoot);
        }
    }
}