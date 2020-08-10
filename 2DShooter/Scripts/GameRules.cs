using Godot;

public class GameRules : Node
{
    public bool engineScaleCheck = false;
    public bool enableSlowMo = false;
    public float currentTime;
    public float slowMoStartTime;
    public float slowMoWeight;
    public float slowMoScale;


    private InputHandler inputHandler = new InputHandler();

    public override void _EnterTree()
    {
        if (!OS.WindowFullscreen)
            OS.WindowFullscreen = true;
    }

    public virtual void PauseGame()
    {
        GetTree().Paused = true;
    }

    public virtual void ResumeGame()
    {
        GetTree().Paused = false;
    }

    async public void EngineScaleCheck()
    {
        // Check if Engine scale is ok
        if (engineScaleCheck == true)
        {
            if (Engine.TimeScale < 1.0f)
            {
                await ToSignal(GetTree().CreateTimer(2f), "timeout");
                Engine.TimeScale = 1.0f;
                engineScaleCheck = false;
            }
        }
    }

    protected void SlowMo(float delta)
    {
        if (currentTime <= 0)
            Engine.TimeScale = Mathf.Lerp(Engine.TimeScale, 1.0f, slowMoWeight);
        else
        {
            Engine.TimeScale = Mathf.Lerp(Engine.TimeScale, slowMoScale, slowMoWeight);
            currentTime -= delta;
        }
    }

    public void StartSlowMotion(float time, float scale, float weight)
    {
        enableSlowMo = true;

        slowMoStartTime = time;
        slowMoWeight = weight;
        slowMoScale = scale;

        currentTime = slowMoStartTime;
    }

    private void OnSlowMoTimerTimeout()
    {
        enableSlowMo = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (inputHandler.FullScreenPressed() && !OS.WindowFullscreen)
            OS.WindowFullscreen = true;
        else if (inputHandler.FullScreenPressed() && OS.WindowFullscreen)
            OS.WindowFullscreen = false;
    }

    public  bool IsPaused => GetTree().Paused;
}