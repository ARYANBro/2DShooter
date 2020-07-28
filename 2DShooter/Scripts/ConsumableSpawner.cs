using Godot;
using System;

public class ConsumableSpawner
{
    private PackedScene healthPackScene;
    private PackedScene energyDrinkScene;
    private Node parent;

    public Consumable InitConsumables(PackedScene consumableScene)
    {
        return consumableScene.Instance() as Consumable;
    }

    public void Spawn<T>(ref T consumable, Vector2 globalPosition, float rotationDegrees, Node parent) where T : Consumable
    {
        consumable.GlobalPosition = globalPosition;
        consumable.RotationDegrees = rotationDegrees;

        parent.AddChild(consumable);
    }

    public ConsumableSpawner(PackedScene _heathPackScene, PackedScene _energyDrinkScene, Node _parent) =>
        (healthPackScene, energyDrinkScene, parent) = (_heathPackScene, _energyDrinkScene, _parent);

}
