using Godot;
using System;

public class Inventory : Node2D
{
    private PackedScene Pickable;

    public void AddItemToInventory(IPickable _pickable)
    {
        Pickable = GD.Load<PackedScene>(_pickable.path);
        if (Pickable != null && GetChildCount() == 0)
        {
            Node2D pickable = Pickable.Instance() as Node2D;
            HideForSeconds(0.02f, pickable, GetTree());

            CallDeferred("add_child", pickable, true);
        }
    }

    public override void _Process(float delta)
    {
        if (GetChildCount() == 2)
        {
            var gun = GetChild(0) as Gun;
            gun.UnEquip();
        }
    }

    public void RemoveItemFromInventory(IPickable pickable)
    {
        foreach (Node child in GetChildren())
        {
            if (pickable.GetType().Name == child.GetType().Name && !child.IsQueuedForDeletion())
                child.CallDeferred("queue_free");
        }
    }

    public static void HideForSeconds(float sec, Node2D node2D, SceneTree sceneTree)
    {
        node2D.Hide();
        sceneTree.CreateTimer(sec).Connect("timeout", node2D, "show");
    }
}