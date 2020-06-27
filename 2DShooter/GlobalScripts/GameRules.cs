using System;
using Godot;

// Game Rules
public class GameRules : Node2D
{
	private void OnPlayerDied() => GetTree().Quit();

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_cancel") && OS.WindowFullscreen)
			OS.WindowFullscreen = false;
		else if (Input.IsActionJustPressed("ui_cancel") && !OS.WindowFullscreen)
			OS.WindowFullscreen = true;

		if (Input.IsActionJustPressed("Reload"))
			GetTree().ReloadCurrentScene();
	}
}
