using Godot;
using System;

public class Score : Label
{
    private int points = 0;

    public override void _Process(float delta)
    {
        GD.Print("[Points] : " + points);
        Text = Convert.ToString(points);
    }

    private void IncreasePoints(int _points)
    {
        points += _points;
    }
}
