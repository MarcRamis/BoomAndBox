using UnityEngine;

public class SoundtrackDaniSong : SoundtrackManager
{
    MTimer loopSongTimer;

    private void Awake()
    {
        loopSongTimer = new MTimer();
        loopSongTimer.SetTimeLimit(2f);
        loopSongTimer.OnTimerEnd += LoopSong;
    }

    private void Start()
    {
        StartSong();
        loopSongTimer.StartTimer();
    }

    private void Update()
    {
        loopSongTimer.Update(Time.deltaTime);
    }

    public override void RythmOn()
    {
    }

    public override void RythmOff()
    {
    }

    public override void StartConfiguration()
    {
        SetInstrumentOff(audioSources[2]);
        SetInstrumentOff(audioSources[3]);
        SetInstrumentOff(audioSources[4]);

        NextConfiguration();

        loopSongTimer.StartTimer();
    }

    private void StartSong()
    {
        SetInstrumentOff(audioSources[2]);
        SetInstrumentOff(audioSources[3]);
        SetInstrumentOff(audioSources[4]);

        SetAudioVolume(audioSources[0], maxVolume);
        SetAudioVolume(audioSources[1], 0.3f);
    }
    private void LoopSong()
    {
        SetAudioVolume(audioSources[2], 0.3f);
        SetAudioVolume(audioSources[3], 0.3f);
        SetAudioVolume(audioSources[4], 0.3f);
    }

    public override void NextConfiguration()
    {
        base.NextConfiguration();
        
        switch (currentIteration)
        {
            case 1:
                
                SetAudioVolume(audioSources[0], maxVolume);
                SetAudioVolume(audioSources[1], 0.2f);
                
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                break;
        }
    }
}