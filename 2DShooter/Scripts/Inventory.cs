using Godot;

public class Inventory : Node2D
{
    public void AddItemToInventory(PackedScene weaponScene)
    {
        if (weaponScene != null && GetChildCount() == 0)
        {
            Node2D pickable = weaponScene.Instance() as Node2D;
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
                child.QueueFree();
        }
    }

    public static void HideForSeconds(float sec, Node2D node2D, SceneTree sceneTree)
    {
        node2D.Hide();
        sceneTree.CreateTimer(sec).Connect("timeout", node2D, "show");
    }

    public bool Has<T>() where T : IPickable
    {   
        if (GetChildCount() == 1 && GetChild(0) is T)
            return true;
        else
            return false;
    }
}