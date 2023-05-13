using UnityEngine;

[System.Serializable]
public class Instrument
{
    public AudioSource instrumentRef;
    [HideInInspector] public float[] audioSamples = new float[512]; // Array para almacenar los datos de audio

    public float threshold = 10;
    public float multiplierNeeded = 100000;
    public float intensity = 0;

    public Beating beating;

    public bool IsIntensityGreater() { return (intensity * instrumentRef.volume) > (threshold * instrumentRef.volume); }

    public void UpdateSpectrumIntensity()
    {
        if (instrumentRef != null)
        {
            intensity = SpectrumOperations.CalculateSpectrumIntensity(this);
        }
    }
    
    public void SetAudioVolume(float volume)
    {
        instrumentRef.volume = volume;
    }
}
