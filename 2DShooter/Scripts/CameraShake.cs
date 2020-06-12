using System;
using Godot;

public class CameraShake : Camera2D
{

    [Export] public float randomness = 3.0f;
    [Export] public float speed = 0.8f;
    [Export] public float decay = 0.6f;
    [Export] public bool shake = true;

    public Timer timer;

    private RandomNumberGenerator randNumGenerator = new RandomNumberGenerator();

    public override void _Ready()
    {
        randNumGenerator.Randomize();
        timer = GetNode<Timer>("Shake Timer");
    }

    public override void _Process(float delta)
    {
        if (!shake)
            Offset = Offset.MoveToward(Vector2.Zero, decay * delta);

        float x = randNumGenerator.RandfRange(-1.0f, 1.0f) * randomness;
        float y = randNumGenerator.RandfRange(-1.0f, 1.0f) * randomness;
        Offset = Offset.MoveToward(new Vector2(x, y), delta * speed);

    }
}
