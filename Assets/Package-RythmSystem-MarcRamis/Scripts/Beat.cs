using UnityEngine;

[System.Serializable]
public class Beat
{
    public delegate void BeatEvent();
    public event BeatEvent OnBeat;
    public bool canRythm = false;
    public float cd = 0.5f;
    MTimer rythmTimer;

    public Beat()
    {
        rythmTimer = new MTimer(cd);
        rythmTimer.OnTimerEnd += ResetRythm;
    }

    public void Update(bool isBeat)
    {
        if (isBeat)
        {
            if (!canRythm)
            {
                rythmTimer.StartTimer();
                OnBeat?.Invoke();
                canRythm = true;
            }
        }

        rythmTimer.Update(Time.deltaTime);
    }

    private void ResetRythm()
    {
        canRythm = false;
    }
}