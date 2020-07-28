using Godot;
using System;

public class EnergyDrink : Consumable
{
	[Export] public int increaseStaminaBy;
	public override Player Player { get; set; }

	public override void _Ready()
	{
		Player = GetTree().CurrentScene.GetNode<Player>("Player");
 	}

	private void OnStaminapackBodyEntered(object body)
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
