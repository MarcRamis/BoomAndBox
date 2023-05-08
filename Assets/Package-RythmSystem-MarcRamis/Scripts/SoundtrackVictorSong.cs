using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackVictorSong : SoundtrackManager
{
    public List<AudioSource> introSong;
    public List<AudioSource> loopSong;

    private bool isLoopSong = false;

    public override void InitializeSequence()
    {
        base.InitializeSequence();

        introSong.Add(audioSources[2]);
        introSong.Add(audioSources[3]);

        loopSong.Add(audioSources[0]);
        loopSong.Add(audioSources[1]);

        audioSources[0].Stop();
        audioSources[1].Stop();

        audioSources[2].Stop();
        audioSources[3].Stop();
        audioSources[2].Play();
        audioSources[3].Play();
    }

    public override void UpdateSequence()
    {
        if (isLoopSong)
        {
            CheckIfLoopFinalized();
        }
        else
        {
            CheckIfIntroFinalized();
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

    public override void CheckIfMusicFinalized()
    {
    }

    public override void ReloadSong()
    {
        audioSources[0].Play();
        audioSources[1].Play();
    }

    private void PlayLoopSong()
    {
        audioSources[0].Play();
        audioSources[1].Play();
        isLoopSong = true;
    }
}