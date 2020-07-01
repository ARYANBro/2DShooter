using Godot;
using System;
using System.Resources;

public class Healthpack : Area2D
{
	[Export] public int increaseHpBy;
	public Player player;

	public override void _Ready()
	{
		GlobalPosition = Utlities.RandPosition(new Vector2(320, 180), GlobalPosition);
		player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	private void OnHealthpackBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			if (player.Hp < 100)
			{
				player.Hp += increaseHpBy;
				QueueFree();
			}
		}
	}
}
