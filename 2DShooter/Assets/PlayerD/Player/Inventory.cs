using Godot;

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

            AddChild(pickable, true);
        }
    }

    public void RemoveItemFromInventory(IPickable pickable)
    {
        foreach (Node child in GetChildren())
        {
            if (pickable.GetType().Name == child.GetType().Name)
                child.QueueFree();
        }
    }

    public static void HideForSeconds(float sec, Node2D node2D, SceneTree sceneTree)
    {
        node2D.Hide();
        sceneTree.CreateTimer(sec).Connect("timeout", node2D, "show");
    }
}