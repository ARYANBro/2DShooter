using Godot;
using System;

public class StaminaBar : TextureProgress
{
	public Player player;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	public override void _Process(float delta)
	{
		Value = Mathf.Lerp((float)Value, player.Stamina, 0.8f);
	}
}
