using Godot;
using System;
using System.Runtime.InteropServices;

public class StaminaBar : TextureProgress
{
	public Player player;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

	public override void _Process(float delta)
	{
		Value = player.stamina;
		Value = Mathf.Lerp((float)Value, player.stamina, 0.5f);
	}
}
