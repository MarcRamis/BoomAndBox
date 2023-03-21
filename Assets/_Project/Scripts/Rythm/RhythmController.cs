using UnityEngine;

public class RhythmController : MonoBehaviour
{
    public AudioSource song;
    public GameObject objectToControl;

    private float[] samples;
    private float[] spectrum;
    private int sampleCount;

    public int lowFrequency = 30;
    public int highFrequency = 300;
    public float scaleMultiplier = 100;

    private void Start()
    {
        // Inicializa arrays de muestras y espectro
        sampleCount = 1024;
        samples = new float[sampleCount];
        spectrum = new float[sampleCount];
    }

    private void Update()
    {
        // Extrae muestras y espectro de la canción
        song.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        song.GetOutputData(samples, 0);

        // Calcula el promedio de la intensidad de las muestras en el rango de frecuencias de la baja
        float average = 0f;
        int sampleRange = Mathf.RoundToInt(sampleCount * highFrequency / AudioSettings.outputSampleRate);
        for (int i = Mathf.RoundToInt(sampleCount * lowFrequency / AudioSettings.outputSampleRate); i < sampleRange; i++)
        {
            average += spectrum[i];
        }
        average /= sampleRange - Mathf.RoundToInt(sampleCount * lowFrequency / AudioSettings.outputSampleRate);

        // Controla la escala del objeto en función de la intensidad promedio de la baja
        objectToControl.transform.localScale = Vector3.one * average * scaleMultiplier;
    }
}