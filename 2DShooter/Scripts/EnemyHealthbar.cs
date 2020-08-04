using Godot;
using System;

public class EnemyHealthbar : TextureProgress
{
	public Enemy enemy { get; private set; }

	public override void _Ready()
	{
		if (GetParent().GetType().Name == "Enemy")
		{
			enemy = GetParent<Enemy>();
		}
		else if (GetParent().GetType().Name == "BigEnemy")
		{
			enemy = GetParent<BigEnemy>();
		}
		else if (GetParent().GetType().Name == "AcidEnemy")
		{
			enemy = GetParent<AcidEnemy>();
		}
		
	}

	public override void _Process(float delta)
	{
		Value = Mathf.Lerp((float)Value, enemy.Hp, 0.5f);
	}
}