using System;
using Godot;

public class Healthbar : TextureProgress
{
	public Player player;

	public override void _Ready()
	{
		player =  GetTree().CurrentScene.GetNode<Player>("Player");
	}

	public override void _Process(float delta)
	{
		if (player != null)
			Value = Mathf.Lerp((float)Value, player.Hp, 0.5f);
	}
}
