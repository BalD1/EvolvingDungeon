using StdNounou;
using System;

public class Timer : ITickable, IDisposable
{
    private float maxDuration;
    private float duration;
    private int remainingTicks;

    private event Action onEnd;

    private bool isSubscribed;

    public Timer(float duration, Action onEnd)
    {
        this.maxDuration =  this.duration = duration;
        this.onEnd = onEnd;
        remainingTicks = (int)(duration / TickManager.TICK_TIMER_MAX);
    }

    private void Subscriber()
    {
        if (isSubscribed) return;
        TickManagerEvents.OnTick += OnTick;
        isSubscribed = true;
    }
    private void Unsubscriber()
    {
        TickManagerEvents.OnTick -= OnTick;
        isSubscribed = false;
    }

    public void Dispose()
    {
        Unsubscriber();
    }

    public void Start()
        => Subscriber();

    public void Stop()
        => Unsubscriber();

    public void Reset()
        => Reset(maxDuration);
    private void Reset(float newTime)
    {
        this.duration = newTime;
        this.remainingTicks = (int)(duration / TickManager.TICK_TIMER_MAX);
        if (!isSubscribed) Subscriber();
    }

    public void OnTick(int tick)
    {
        remainingTicks--;
        duration -= TickManager.TICK_TIMER_MAX;

        if (remainingTicks <= 0) OnEnd();
    }

    public int RemainingTicks()
        => remainingTicks;

    public float RemainingTimeInSeconds()
        => duration;

    private void OnEnd()
    {
        onEnd?.Invoke();
        Unsubscriber();
    }
}
