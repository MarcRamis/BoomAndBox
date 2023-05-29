using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundtracks { FIRST, SECOND, COUNT}
public enum ERythmMode { SCHEDULED, FREE }

public class RythmController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public SoundtrackManager soundtrackManager;
    [SerializeField] public List<SoundtrackManager> soundtracks;
    
    [Header("Settings")]
    [SerializeField] private ESoundtracks soundtrackState = ESoundtracks.FIRST;

    public ERythmMode rythmMode;
    public static RythmController instance;
    
    public Beat beat;
    
    public delegate void OnScheduledModeEvent();
    public OnScheduledModeEvent OnScheduledMode;

    public delegate void OnFreeModeEvent();
    public OnFreeModeEvent OnFreeMode;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        beat = new Beat();
        
    }
    
    private void Start()
    {
        HandleRythmMode(rythmMode);
        soundtrackManager.InitializeSoundtracks();
    }

    private void Update()
    {
        soundtrackManager.UpdateSoundtracks();

        ManageInputs();

        beat.Update(soundtrackManager.GetAllInstruments());

    }

    private void ManageInputs()
    {
        // Cambiar la canción
        if (Input.GetKeyDown(KeyCode.P))
        {
            soundtrackState += 1;
            if (soundtrackState == ESoundtracks.COUNT)
            {
                soundtrackState = ESoundtracks.FIRST;
            }
            SetNewState(soundtrackState);
        }
        
        // Cambiar el modo de juego
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (rythmMode == ERythmMode.SCHEDULED)
            {
                rythmMode = ERythmMode.FREE;
            }
            else
            {
                rythmMode = ERythmMode.SCHEDULED;
            }
            SetNewMode(rythmMode);
        }
    }
    

    private void HandleSoundtrack(ESoundtracks newState)
    {
        soundtrackManager.StopSoundtracks();
        switch (newState)
        {
            case ESoundtracks.FIRST:

                soundtrackManager = soundtracks[0];
                SimonController.instance.SetSequenceController(0); 

                break;
                
            case ESoundtracks.SECOND:
                
                soundtrackManager = soundtracks[1];
                SimonController.instance.SetSequenceController(1);

                break;

            case ESoundtracks.COUNT:
                break;
        }

        soundtrackManager.InitializeSoundtracks();
    }


    private void HandleRythmMode(ERythmMode newMode)
    {
        SetNewState(soundtrackState);

        switch(newMode)
        {
            case ERythmMode.SCHEDULED:
                
                SimonController.instance.StopSimon();
                soundtrackManager.ScheduledInitialize();
               
                OnScheduledMode?.Invoke();

                break;

            case ERythmMode.FREE:
                
                SimonController.instance.Initialize();
                soundtrackManager.FreedInitialize();
               
                OnFreeMode?.Invoke();

                break;
        }
    }
    
    private void SetNewMode(ERythmMode newMode)
    {
        rythmMode = newMode;
        HandleRythmMode(rythmMode);
    }

    private void SetNewState(ESoundtracks newState)
    {
        soundtrackState = newState;
        HandleSoundtrack(soundtrackState);
    }
    
}
