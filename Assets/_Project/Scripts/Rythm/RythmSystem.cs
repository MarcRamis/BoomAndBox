using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmSystem : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] audioSamples = new float[512]; // Array para almacenar los datos de audio

    public float threshold = 0.1f; // Umbral para detectar el ritmo
    public float speed = 1f; // Velocidad del movimiento
    public float maxDistance = 2f; // Distancia máxima del movimiento
    public float multiplierNeeded = 10000f; // Multiplier que utilizo para mejorar el valor de intensidad que obtengo del volumen del audio, para tener más control a la hora de hacer que se mueva
    
    private void Awake()
    {
        audioSource = GameObject.FindGameObjectWithTag("MainSoundtrack").GetComponent<AudioSource>();

        if (multiplierNeeded < 1)
            multiplierNeeded = 1;
    }

    private void Update()
    {
        // Obtener los datos de audio del audio source
        audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.BlackmanHarris);

        // Sumar los valores absolutos de las muestras de audio para obtener una medida de la intensidad del ritmo
        float sum = 0f;
        for (int i = 0; i < audioSamples.Length; i++)
        {
            sum += Mathf.Abs(audioSamples[i]);
        }
        float intensity = sum / audioSamples.Length;
        intensity *= multiplierNeeded;
       
        // Si la intensidad del ritmo supera el umbral, mover el objeto hacia arriba
        if (intensity > threshold)
        {
            Debug.Log(intensity);
            transform.position += Vector3.up * speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Min(transform.position.y, maxDistance), transform.position.z);
        }
    }
}
