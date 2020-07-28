using Godot;
using System;

public class Score : Label
{
    public AnimationPlayer animPlayer;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("Score Animplayer");
    }

    private int points = 0;

    public override void _Process(float delta)
    {
        Text = Convert.ToString(points) + "P";
    }

    private void IncreasePoints(int _points)
    {
        points += _points; 
        animPlayer.Play("Points bounce");
    }
}