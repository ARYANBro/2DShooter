using Godot;

public class InputHandler : Node
{
    public Vector2 GetInputVector()
    {
        Vector2 inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        inputVector.y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
        inputVector = inputVector.Normalized();

        return inputVector;
    }

    public bool SprintPressed(MovementHandler movementHandler)
    {
        return Input.IsActionPressed("Sprint") && movementHandler.canSprint && movementHandler.velocity != Vector2.Zero;
    }
    
    public bool UnEquipPressed(Gun gun)
    {
        return Input.IsActionJustPressed("UnEquip") && gun.isEquiped;
    }

    public bool EquipedPressed(Gun gun)
    {
        return Input.IsActionJustPressed("Equip") && gun.wantToEquipGun;
    }

    public bool FullScreenPressed()
    {
        return Input.IsActionJustPressed("FullScreen");
    }
    
    public bool PausedPressed()
    {
        return Input.IsActionJustPressed("Pause");
    }
}