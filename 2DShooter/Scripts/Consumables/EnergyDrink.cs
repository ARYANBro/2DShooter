using Godot;
using System;

public class EnergyDrink : Consumable
{
	[Export] public int increaseStaminaBy;

	private void OnStaminapackBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			if (Player.movementHandler.Stamina < 400)
			{	
				Player.movementHandler.Stamina += increaseStaminaBy;
				QueueFree();
			}
		}
	}
}
