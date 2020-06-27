using System;
using Godot;

public class Healthbar : TextureProgress
{
	[Signal] public delegate void PlayerDied();

	public Player player;

	public override void _Ready()
	{
		player =  GetTree().CurrentScene.GetNode<Player>("Player");
	}

	public override void _Process(float delta)
	{
		Value = Mathf.Lerp((float)Value, player.Hp, 0.5f);

		if (Value <= 2)
			EmitSignal("PlayerDied");
	}
}
