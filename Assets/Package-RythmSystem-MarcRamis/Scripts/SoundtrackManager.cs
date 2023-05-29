using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    [SerializeField] protected Instrument[] instruments; // this are the instruments that will calculate the spectrum
    [SerializeField] protected List<Instrument> duplicatedInstruments; // This are the instruments that will be heared by the player
    [SerializeField] protected int freedInstrumentIndx; // This is the index of the instrument that is always played on the free mode

    [HideInInspector] protected float maxVolume = 0.5f;
    [HideInInspector] protected float minVolume = 0f;
    [HideInInspector] public int currentIteration = 0;
    
    public virtual void InitializeSoundtracks()
    {
        foreach(Instrument i in instruments)
        {
            if (i.instrumentRef != null)
            {
                i.instrumentRef.volume = 0.001f;
            }
        }

        ReloadSong();

        currentIteration = 0;
    }

    public virtual void StopSoundtracks()
    {
        foreach (Instrument i in instruments)
        {
            if (i.instrumentRef != null)
            {
                i.instrumentRef.Stop();
            }
        }
        foreach (Instrument i in duplicatedInstruments)
        {
            if (i.instrumentRef != null)
            {
                i.instrumentRef.Stop();
            }
        }
    }

    public virtual void StartSongLater()
    {
        foreach (Instrument i in duplicatedInstruments)
        {
            if (i.instrumentRef != null)
            {
                i.instrumentRef.Play();
            }
        }
    }

    public virtual void UpdateSoundtracks()
    {
        CheckIfMusicFinalized();

        foreach (Instrument i in instruments)
        {
            if (i.instrumentRef != null)
            {
                i.UpdateSpectrumIntensity();
            }
        }
    }

    public virtual void RythmOn()
    {
    }
    public virtual void RythmOff()
    {
    }
    
    public virtual void ScheduledInitialize()
    {
        AllBeating();
    }
    
    public virtual void FreedInitialize()
    {
        NoBeating();
        SelectInstrumentToBeat(freedInstrumentIndx, 0.6f);
    }
    public virtual void ConfigurateFinal()
    {
        // Called when the secuence in the simon game is finished
    }
    public void RestSyncInstrument()
    {
        for (var i = instruments.Length - 1; i >= 0; i--)
        {
            if (instruments[i] != instruments[0] && instruments[i] != instruments[freedInstrumentIndx] && instruments[i].isBeating == true)
            {
                SelectInstrumentToStopBeating(i);
                return;
            }
        }

    }
    
    protected virtual void ReloadSong()
    {
        foreach (Instrument i in instruments)
        {
            if (i.instrumentRef != null)
            {
                i.instrumentRef.Play();
            }
        }
        Invoke(nameof(StartSongLater), 0.005f);
    }

    public virtual void RythmOnFreed()
    {
        SelectFollowingInstrument(1f);
    }

    public void SelectFollowingInstrument(float volume) 
    {
        for (int i = 0; i < instruments.Length; i++)
        {
            if (instruments[i].instrumentRef != null)
            {
                if (instruments[i].isBeating == false)
                {
                    SelectInstrumentToBeat(i, volume);
                    break;
                }
            }

        }
    }
    
    public void SelectConfiguration(int indx)
    {
        // Sum aconfiguration is called every time secuence controller of the simon game changes his current secuence to the next
        currentIteration = indx;
    }

    protected virtual void CheckIfMusicFinalized()
    {
        foreach (Instrument i in instruments)
        {
            if (i.instrumentRef != null)
            {
                if (i.instrumentRef.isPlaying)
                {
                    return;
                }
                else
                {
                    ReloadSong();
                }
            }
        }
    }

    protected void NoBeating()
    {
        foreach (Instrument i in instruments)
        {
            if (i.instrumentRef != null)
            {
                i.isBeating = false;
            }
        }
        NoVolume();
    }

    protected void AllBeating()
    {
        foreach (Instrument i in instruments)
        {
            if (i.instrumentRef != null)
            {
                i.isBeating = true;
            }
        }
        MaxVolume();
    }

    protected void NoVolume()
    {
        foreach (Instrument i in duplicatedInstruments)
        {
            if (i.instrumentRef != null)
            {
                i.SetAudioVolume(0f);
            }
        }
    }
    protected void MaxVolume()
    {
        foreach (Instrument i in duplicatedInstruments)
        {
            if (i.instrumentRef != null)
            {
                i.SetAudioVolume(1f);
            }
        }
    }
    
    protected void SelectInstrumentToBeat(int index, float volume)
    {
        if (instruments[index].instrumentRef != null)
        {
            instruments[index].isBeating = true;
            duplicatedInstruments[index].SetAudioVolume(volume);
        }
    }
    protected void SelectInstrumentToStopBeating(int index)
    {
        if (instruments[index].instrumentRef != null)
        {
            instruments[index].isBeating = false;
            duplicatedInstruments[index].SetAudioVolume(0);
        }
    }

    public Instrument[] GetAllInstruments() { return instruments; }
}