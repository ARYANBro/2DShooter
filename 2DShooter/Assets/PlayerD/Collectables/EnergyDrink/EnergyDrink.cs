using Godot;
using System;

public class EnergyDrink : Area2D
{
	[Export] public int increaseStaminaBy;
	public Player player;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	private void OnStaminapackBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			if (player.Stamina < 400)
			{
				player.Stamina += increaseStaminaBy;
				QueueFree();
			}
		}
	}
}
