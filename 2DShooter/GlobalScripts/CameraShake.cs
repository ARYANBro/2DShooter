using System;
using Godot;

public class CameraShake : Camera2D
{
    [Export] public float randomness = 3.0f;
    [Export] public float speed = 1.5f;
    [Export] public float decay = 0.6f;
    [Export] public bool shake = true;

    private Vector2 orignalPos;

    public Timer shakeTimer;

    private RandomNumberGenerator randNumGenerator = new RandomNumberGenerator();

    public override void _Ready()
    {
        randNumGenerator.Randomize();
        shakeTimer = GetNode<Timer>("Shake Timer");
        orignalPos = Offset;
    }

    public override void _Process(float delta)
    {
        if (shake)
        {
            float x = randNumGenerator.RandfRange(-1.0f, 1.0f) * randomness;
            float y = randNumGenerator.RandfRange(1.0f, -1.0f) * randomness;
            Offset = Offset.MoveToward(new Vector2(x, y), delta * speed);
        }
        else
            Offset = Offset.MoveToward(orignalPos, decay * delta);
    }

    public void StartShake()
    {
        shake = true;
        shakeTimer.Start();
    }

    public void Shake(float _randomness, float _speed, float _decay)
    {
        randomness = _randomness;
        speed = _speed;
        decay = _decay;
        shake = true;
        shakeTimer.Start();
    }

    private void OnShakeTimerTimeout()
    {
        shake = false;
    }
}
