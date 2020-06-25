using Godot;
using System;

public class Healthpack : Area2D
{
	[Export]
	public int increaseHpBy;
	public Player player;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	private void OnHealthpackBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			if (player.hp < 100)
			{
				player.hp += increaseHpBy;
				QueueFree();
			}

		}
	}
}