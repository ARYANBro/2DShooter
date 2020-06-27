using Godot;
using System;

public class EnemyHealthbar : TextureProgress
{
	public Enemy enemy;

	public override void _Ready()
	{
		enemy = GetParent<Enemy>();
	}

	public override void _Process(float delta)
	{
		Value = Mathf.Lerp((float)Value, enemy.Hp, 0.5f);
	}
}
