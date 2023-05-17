using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackGooFunk : SoundtrackManager
{
    public List<AudioSource> introSong;
    public List<AudioSource> loopSong;

    private bool isLoopSong = false;

    public override void InitializeSoundtracks()
    {
        base.InitializeSoundtracks();
        
        baseInstrument = instruments[3];
        
        introSong.Add(instruments[2].instrumentRef);
        introSong.Add(instruments[3].instrumentRef);

        loopSong.Add(instruments[1].instrumentRef);
        loopSong.Add(instruments[4].instrumentRef);

        instruments[1].instrumentRef.volume = 0.7f;
        instruments[4].instrumentRef.volume = 0.15f;
        instruments[2].instrumentRef.volume = 0.6f;
        instruments[3].instrumentRef.volume = 0.1f;
        
        instruments[1].instrumentRef.Stop();
        instruments[4].instrumentRef.Stop();
       
        instruments[2].instrumentRef.Stop();
        instruments[3].instrumentRef.Stop();
        instruments[2].instrumentRef.Play();
        instruments[3].instrumentRef.Play();
    }

    public override void UpdateSoundtracks()
    {
        if (isLoopSong)
        {
            CheckIfLoopFinalized();
        }
        else
        {
            CheckIfIntroFinalized();
        }

        foreach (Instrument i in instruments)
        {
            i.UpdateSpectrumIntensity();
        }

        foreach (Instrument i in instruments)
        {
            i.beating.UpdateBeating();
        }
    }

    private void CheckIfIntroFinalized()
    {
        foreach (AudioSource audioSource in introSong)
        {
            if (audioSource.isPlaying)
            {
                return;
            }
            else
            {
                baseInstrument = instruments[4];
                PlayLoopSong();
            }
        }
    }

    private void CheckIfLoopFinalized()
    {
        foreach (AudioSource audioSource in loopSong)
        {
            if (audioSource.isPlaying)
            {
                return;
            }
            else
            {
                ReloadSong();
            }
        }
    }

    protected override void CheckIfMusicFinalized()
    {
    }

    protected override void ReloadSong()
    {
        instruments[1].instrumentRef.Play();
        instruments[4].instrumentRef.Play();
    }

    private void PlayLoopSong()
    {
        instruments[1].instrumentRef.Play();
        instruments[4].instrumentRef.Play();
        isLoopSong = true;
    }
}