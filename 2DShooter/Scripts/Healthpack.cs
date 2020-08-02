using Godot;
using System;
using System.Resources;

public class Healthpack : Consumable
{
	[Export] public int increaseHpBy;

	void OnHealthpackBodyEntered(object body)
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
