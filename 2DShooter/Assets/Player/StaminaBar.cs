using Godot;
using System;

public class StaminaBar : TextureProgress
{
	public Player player;

	public override void _Ready()
	{
		player = (Player)GetTree().CurrentScene.FindNode("Player", true, true);
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("Sprint") && player.canSprint)
			Value -= 3;
		else if (!player.canSprint || !Input.IsActionPressed("Sprint"))
			Value += 1;

		Value = Mathf.Clamp((int)Value, 0, 400);

		if (Value <= 0)
			player.canSprint = false;
		else if (Value >= 400)
			player.canSprint = true;
	}
}
