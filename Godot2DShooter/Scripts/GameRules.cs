using Godot;
using System;

public class GameRules : Node
{
	/*
	public override void _Process(float delta)
	{
		
		if (Input.IsActionPressed("ui_cancel"))
			GetTree().Quit();
		
		if (Input.IsActionJustPressed("Reload Scene"))
		{
			GetTree().ReloadCurrentScene();
		}
	}
	*/

	public override void _Ready()
	{
		SetPhysicsProcess(true);

		Input.SetMouseMode(Input.MouseMode.Captured);
	}

	public override void _PhysicsProcess(float delta)
	{
		Inputs();
	}

	private void Inputs()
	{
		if (Input.IsActionJustPressed("ui_cancel") && Input.GetMouseMode() == Input.MouseMode.Captured)
		{
			Input.SetMouseMode(Input.MouseMode.Visible);
		}
		else if (Input.IsActionJustPressed("ui_cancel") && Input.GetMouseMode() == Input.MouseMode.Visible)
		{
			Input.SetMouseMode(Input.MouseMode.Captured);
		}

		if (Input.IsActionJustPressed("Reload Scene"))
		{
			GetTree().ReloadCurrentScene();
		}
	}
}
