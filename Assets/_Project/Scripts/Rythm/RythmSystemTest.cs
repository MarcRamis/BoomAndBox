using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERythmBase { NONE, FIRST, SECOND, THIRD }

public class RythmSystemTest : MonoBehaviour
{ 
    [HideInInspector] private AudioSource audioSource;
    [HideInInspector]  private float[] audioSamples = new float[512]; // Array para almacenar los datos de audio

    [Header("Settings")]
    [SerializeField] private float threshold = 0.1f; // Umbral para detectar el ritmo
    [SerializeField] private float multiplierNeeded = 10000f; // Multiplier que utilizo para mejorar el valor de intensidad que obtengo del volumen del audio, para tener más control a la hora de hacer que se mueva
    public ERythmBase rythmState = ERythmBase.NONE;

    [Header("AudioClips Container")]
    [SerializeField] private AudioClip[] audioBase;

    private float intensity;
    
    private void Awake()
    {
        GameObject loopMusic = GameObject.FindGameObjectWithTag("MainSoundtrack");

        if (loopMusic != null)
        {
            audioSource = loopMusic.GetComponent<AudioSource>();
            SetNewState(ERythmBase.THIRD);
        }
    }
    
    private void Update()
    {
        if (multiplierNeeded < 1)
            multiplierNeeded = 1;

        if (Input.GetKeyDown(KeyCode.P))
        {
            SetNewState(ERythmBase.FIRST);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetNewState(ERythmBase.SECOND);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetNewState(ERythmBase.THIRD);
        }
    }

    private void FixedUpdate()
    {
        // Obtener los datos de audio del audio source
        audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.BlackmanHarris);

        // Sumar los valores absolutos de las muestras de audio para obtener una medida de la intensidad del ritmo
        float sum = 0f;
        for (int i = 0; i < audioSamples.Length; i++)
        {
            sum += Mathf.Abs(audioSamples[i]);
        }
        intensity = sum / audioSamples.Length;
        intensity *= multiplierNeeded;

        if (IsRythmMoment())
        {
            //Debug.Log(intensity);
        }
    }

    private void HandleRythmState()
    {
        audioSource.Stop();

        switch (rythmState)
        {
            case ERythmBase.NONE:
                break;
            case ERythmBase.FIRST:
                audioSource.clip = audioBase[0];
                break;
            case ERythmBase.SECOND:
                audioSource.clip = audioBase[1];
                break;
            case ERythmBase.THIRD:
                audioSource.clip = audioBase[2];
                break;
        }

        audioSource.Play();
    }

    private void SetNewState(ERythmBase newState)
    {
        rythmState = newState;
        HandleRythmState();
    }

    public bool IsRythmMoment()
    {
        return intensity > threshold;
    }
}