using Godot;
using System;

public class EnergyDrink : Consumable
{
	[Export] public int increaseStaminaBy;

	void OnStaminapackBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			if (Player.Stamina < 400)
			{	
				Player.Stamina += increaseStaminaBy;
				QueueFree();
			}
		}
	}
}
