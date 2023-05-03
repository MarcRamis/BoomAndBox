using System;
using System.Collections.Generic;
using UnityEngine;

// this controls corresponds to the ps4 controller but xbox could be the same
public enum EControlType { NONE, SQUARE, CROSS, TRIANGLE, CIRCLE, UP, DOWN, RIGHT, LEFT}

public class SoundtrackManager : MonoBehaviour
{
    // this instruments are the list to contain every audioSource
    // is separated from the button sequence because i want the button sequence to be modificable on inspector
    // and the audio source need to be created and cleaned every time a song is played
    [SerializeField] protected List<AudioSource> audioSources; 
    
    // those are the audios of every instrument that is played in the song
    [SerializeField] protected AudioClip[] audios; 
    
    [SerializeField] protected ButtonsSequence[] buttonSequences; // all the sequence that will be played in this song
    [SerializeField] protected ButtonsSequence currentSequence; // this only exists to get the current sequence of buttons that will be played
    [SerializeField] protected ButtonsSequence baseSequence;
    
    [HideInInspector] protected float maxVolume = 0.5f;
    [HideInInspector] protected float minVolume = 0f;
    [HideInInspector] public int currentIteration = 0;

    public virtual void InitializeSequence()
    {
        bool firstOne = true;

        foreach (ButtonsSequence sq in buttonSequences)
        {
            if (!firstOne)
            {
                sq.SetInitControl();
            }
            else
            {
                firstOne = false;
            }
        }
        
        currentSequence = buttonSequences[1];
        baseSequence = buttonSequences[1];
    }

    public virtual void RythmOn()
    {
        bool firstOne = true;
        
        foreach(AudioSource audioSource in audioSources)
        {
            if (!firstOne)
            {
                audioSource.volume = maxVolume;
            }
            else
            {
                firstOne = false;
            }
        }
    }

    public virtual void StartConfiguration()
    {

    }

    public virtual void NextConfiguration()
    {
        currentIteration++;
    }

    public virtual void RythmOff()
    {
        bool firstOne = true;

        foreach (AudioSource audioSource in audioSources)
        {
            if (!firstOne)
            {
                audioSource.volume = maxVolume;
            }
            else
            {
                firstOne = false;
            }
        }
    }

    public void SetInstrumentOn(AudioSource audioSource)
    {
        audioSource.volume = maxVolume;
    }
    public void SetInstrumentOff(AudioSource audioSource)
    {
        audioSource.volume = minVolume;
    }
    
    public void SetAudioVolume(AudioSource audioSource, float volume)
    {
        audioSource.volume = volume;
    }

    public List<AudioSource> GetInstruments() { return audioSources; }
    public AudioClip[] GetAudios() { return audios; }
    public float GetThreshold() { return currentSequence.instrument.threshold; }
    public float GetMultiplierNeeded() { return currentSequence.instrument.multiplierNeeded; }
    public void AddAudioSource(AudioSource audioSource) { audioSources.Add(audioSource); }
    public void SetInstrumentAudioSource(AudioSource audioSource, int index) 
    {
        // starts at 1 for the inspector error
        for (int i = 1; i < buttonSequences.Length; i++)
        {
            if (i == index + 1)
            {
                buttonSequences[i].instrument.instrumentRef = audioSource;
                break;
            }
        }
    }
    public ButtonsSequence GetCurrentSequence() { return currentSequence; }
    public ButtonsSequence GetBaseSequence() { return baseSequence; }
    public ButtonsSequence[] GetAllButtonsSequence() { return buttonSequences; }
}