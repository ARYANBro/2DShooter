using Godot;

public class Score : Label
{
    public AnimationPlayer animPlayer;
    private int points = 0;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("Score Animplayer");
    }


    public override void _Process(float delta)
    {
        Text = points.ToString() + "P";
    }

    private void IncreasePoints(int _points)
    {
        points += _points; 
        animPlayer.Play("Points bounce");
    }
}