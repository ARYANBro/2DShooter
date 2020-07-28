using Godot;
using System;
using System.Resources;

public class Healthpack : Consumable
{
	[Export] public int increaseHpBy;
	public override Player Player { get; set; }

	public override void _Ready()
	{
		Player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	private void OnHealthpackBodyEntered(object body)
	{
		if (body.GetType().Name == "Player")
		{
			if (Player.Hp < 100)
			{
				Player.Hp += increaseHpBy;
				QueueFree();
			}
		}
	}
}
