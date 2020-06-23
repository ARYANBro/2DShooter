using Godot;
using System;

public class StaminaPack : Area2D
{
	[Export]
	public int increaseStaminaBy;
	public Player player;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	private void OnStaminapackBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			if (player.stamina < 400)
			{
				player.stamina += increaseStaminaBy;
				QueueFree();
			}

		}
	}
}
