using UnityEngine;

public class MTimer
{
    public float timeLimit;
    public bool autoRestart = false;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    public delegate void TimerEvent();
    public event TimerEvent OnTimerEnd;

    public MTimer() { }

    public MTimer(float timeLimit)
    {
        this.timeLimit = timeLimit;
    }

    public void Update(float deltaTime)
    {
        if (isRunning)
        {
            elapsedTime += deltaTime;
            if (elapsedTime >= timeLimit)
            {
                isRunning = false;
                OnTimerEnd?.Invoke();
                if (autoRestart)
                {
                    StartTimer();
                }
            }
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void SetTimeLimit(float timeLimit)
    {
        this.timeLimit = timeLimit;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}