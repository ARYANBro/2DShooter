using System;
using System.Text.RegularExpressions;
using Godot;

public class Healthbar : TextureProgress
{
	[Signal]
	public delegate void PlayerDied();

	public Player player;

	public override void _Ready()
	{
		player =  GetTree().CurrentScene.GetNode<Player>("Player");
		player.Connect("PlayerDamaged", this, "OnPlayerDamaged");
	}
	private void OnPlayerDamaged()
	{
		Value = Mathf.Lerp((float)Value, player.hp, 0.5f);
		
		if (Value <= 2)
			EmitSignal("PlayerDied");
	}
}
