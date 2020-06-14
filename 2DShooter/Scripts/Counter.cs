using System;
using Godot;

public class Counter : Label
{
    private int counter = 0;

    private void OnEnemyDied()
    {
        counter += 10;
        Text = Convert.ToString(counter);
    }
}
