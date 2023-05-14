using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundtracks { FIRST, SECOND, THIRD, FOURTH, COUNT}

public class RythmController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public SoundtrackManager soundtrackManager;
    
    [Header("Settings")]
    [SerializeField] private ESoundtracks soundtrackState = ESoundtracks.FIRST;

    public static RythmController instance;
    public Beat beat;
    public Beat beat2;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;

        beat = new Beat();
        beat2 = new Beat();
    }

    private void Start()
    {
        soundtrackManager.InitializeSoundtracks();
    }

    private void Update()
    {
        soundtrackManager.UpdateSoundtracks();

        //ManageInputs(); // para testear rápido diferentes canciones
        
        beat.Update(soundtrackManager.GetBaseInstrument().IsIntensityGreater());
        beat2.Update(soundtrackManager.GetAllInstruments()[2].IsIntensityGreater());
    }

    private void ManageInputs()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            soundtrackState += 1;
            if (soundtrackState == ESoundtracks.COUNT)
            {
                soundtrackState = ESoundtracks.FIRST;
            }
            SetNewState(soundtrackState);
        }
    }
    

    private void HandleRythmState(ESoundtracks newState)
    {
        switch (newState)
        {
            case ESoundtracks.FIRST:


                break;

            case ESoundtracks.SECOND:

                break;

            case ESoundtracks.THIRD:

                
                break;
                
            case ESoundtracks.FOURTH:


                break;
            case ESoundtracks.COUNT:
                break;
        }
    }
    
    private void NewBase(int index)
    {
        soundtrackManager.SetBaseInstrument(index);
    }

    private void SetNewState(ESoundtracks newState)
    {
        soundtrackState = newState;
        HandleRythmState(soundtrackState);
    }
    
}
