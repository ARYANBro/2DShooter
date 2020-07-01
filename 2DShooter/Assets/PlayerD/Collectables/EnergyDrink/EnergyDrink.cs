using Godot;
using System;

public class EnergyDrink : Area2D
{
	[Export] public int increaseStaminaBy;
	public Player player;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
		GlobalPosition = Utlities.RandPosition(new Vector2(320, 180), GlobalPosition);
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
