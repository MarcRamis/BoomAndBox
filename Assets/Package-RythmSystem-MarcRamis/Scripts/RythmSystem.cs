using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundtracks { FIRST, SECOND, THIRD, FOURTH, COUNT}

public enum ERythmMode { BASE, SIMON}
public enum ESimonMode { EXAMPLE_SIMON, SIMONSAYS }

public class RythmSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public SoundtrackManager soundtrackManager;
    [SerializeField] private GameObject soundtrackPrefab;
    [SerializeField] private GameObject soundtrackParent;
    
    [Header("Settings")]
    [SerializeField] private ESoundtracks soundtrackState = ESoundtracks.FIRST;
    [SerializeField] private ERythmMode rythmMode = ERythmMode.BASE;
    
    [HideInInspector] private AudioSource audioBase;
    [HideInInspector] public AudioSource audioExtraBase;
    [HideInInspector] private float[] audioSamples = new float[512]; // Array para almacenar los datos de audio
    
    public ESimonMode simonMode = ESimonMode.EXAMPLE_SIMON;

    public static RythmSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        SetNewState(soundtrackState);
        //SetNewMode(rythmMode);
    }
    
    private void FixedUpdate()
    {
        CheckIfMusicFinalized();
        ManageInputs(); // para testear rápido diferentes canciones

        HandleRythmMode(rythmMode);


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

                InitSoundtrack(GetComponent<SoundtrackHiFiRush>());

                break;

            case ESoundtracks.SECOND:

                InitSoundtrack(GetComponent<SoundtrackItsFunky>());

                break;

            case ESoundtracks.THIRD:

                InitSoundtrack(GetComponent<SoundtrackZapslat>());
                
                break;
                
            case ESoundtracks.FOURTH:

                InitSoundtrack(GetComponent<SoundtrackDaniSong>());

                break;
            case ESoundtracks.COUNT:
                break;
        }
    }
    
    private void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    
    private void InitSoundtrack(SoundtrackManager _soundtrackManager)
    {
        soundtrackManager = _soundtrackManager;

        ClearChildren(soundtrackParent.transform);
        bool firstOne = true;
        
        for(int i = 0; i < soundtrackManager.GetAudios().Length; i++)
        {
            GameObject gameObject = Instantiate(soundtrackPrefab, soundtrackParent.transform);
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = soundtrackManager.GetAudios()[i];
            audioSource.Play();

            soundtrackManager.AddAudioSource(audioSource);
            soundtrackManager.SetInstrumentAudioSource(audioSource, i);

            if (firstOne)
            {
                audioBase = audioSource;
                firstOne = false;
            }
        }

        soundtrackManager.InitializeSequence();
    }
    
    private void ReloadSong()
    {
        foreach (AudioSource audioSource in soundtrackManager.GetInstruments())
        {
            audioSource.Play();
        }
    }
    
    private void CheckIfMusicFinalized()
    {
        foreach (AudioSource audioSource in soundtrackManager.GetInstruments())
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
    
    private void SetNewState(ESoundtracks newState)
    {
        soundtrackState = newState;
        HandleRythmState(soundtrackState);
    }
    
    public void SetNewMode(ERythmMode newState)
    {
        rythmMode = newState;
    }
    
    private void HandleRythmMode(ERythmMode rythmMode)
    {
        CalculateRythmMoment(soundtrackManager.GetBaseSequence().instrument);

        switch (rythmMode)
        {
            case ERythmMode.BASE:
        
                break;
        
            case ERythmMode.SIMON:
                
                for (int i = 0; i < soundtrackManager.GetAllButtonsSequence().Length; i++)
                {
                    if (i > 1) // inspector bug..
                    {
                        CalculateRythmMoment(soundtrackManager.GetAllButtonsSequence()[i].instrument);
                    }
                }
        
                break;
        }
    }

    private void CalculateRythmMoment(Instrument instrument)
    {
        // Obtener los datos de audio del audio source
        instrument.instrumentRef.GetSpectrumData(audioSamples, 0, FFTWindow.BlackmanHarris);

        // Sumar los valores absolutos de las muestras de audio para obtener una medida de la intensidad del ritmo
        float sum = 0f;
        for (int i = 0; i < audioSamples.Length; i++)
        {
            sum += Mathf.Abs(audioSamples[i]);
        }
        instrument.intensity = sum / audioSamples.Length;
        instrument.intensity *= instrument.multiplierNeeded;
    }

    public bool IsRythmBaseMoment()
    {
        return soundtrackManager.GetBaseSequence().instrument.IsRythmMoment();
    }
    
    public bool IsRythmSimonMoment()
    {
        return soundtrackManager.GetCurrentSequence().instrument.IsRythmMoment();
    }

    public void SetNewSimonMonde(ESimonMode eSimonMode)
    {
        simonMode = eSimonMode;
    }
}