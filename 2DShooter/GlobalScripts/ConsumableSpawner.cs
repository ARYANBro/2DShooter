using Godot;
using System;

public class ConsumableSpawner
{
    private PackedScene healthPackScene;
    private PackedScene energyDrinkScene;
    private Node parent;

    public ConsumableSpawner(PackedScene _heathPackScene, PackedScene _energyDrinkScene, Node _parent) =>
        (healthPackScene, energyDrinkScene, parent) = (_heathPackScene, _energyDrinkScene, _parent);

    public void Spawn()
    {
        int randNum = Utlities.randNumGenerator.RandiRange(0, 1);

        Node healthpack = healthPackScene.Instance();
        Node energyDrink = energyDrinkScene.Instance();

        // Randomly spawn consumables
        if (randNum == 0)
            parent.AddChild(energyDrink);
        else if (randNum == 1)
            parent.AddChild(healthpack);
    }
}
