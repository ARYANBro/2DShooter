using System;
using Godot;

public class CameraShake : Camera2D
{
    [Export] public float randomness = 3.0f;
    [Export] public float speed = 1.5f;
    [Export] public float decay = 0.6f;
    [Export] public float shakeTimerWaitTime;
    public bool shake = false;

    RandomNumberGenerator randNumGenerator = new RandomNumberGenerator();
    Vector2 orignalPos;

    public override void _Ready()
    {
        randNumGenerator.Randomize();
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
        GetTree().CreateTimer(shakeTimerWaitTime).Connect("timeout", this, "OnShakeTimerTimeout");
    }

    public void Shake(float _randomness, float _speed, float _decay)
    {
        randomness = _randomness;
        speed = _speed;
        decay = _decay;
        shake = true;
        GetTree().CreateTimer(shakeTimerWaitTime).Connect("timeout", this, "OnShakeTimerTimeout");
    }

    void OnShakeTimerTimeout() => shake = false;
}