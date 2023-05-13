using UnityEngine;

public static class SpectrumOperations
{
    public static float CalculateSpectrumIntensity(Instrument instrument)
    {
        // Obtener los datos de audio del audio source
        instrument.instrumentRef.GetSpectrumData(instrument.audioSamples, 0, FFTWindow.BlackmanHarris);

        // Sumar los valores absolutos de las muestras de audio para obtener una medida de la intensidad del ritmo
        float sum = 0f;
        for (int i = 0; i < instrument.audioSamples.Length; i++)
        {
            sum += Mathf.Abs(instrument.audioSamples[i]);
        }

        instrument.intensity = sum / instrument.audioSamples.Length;
        instrument.intensity *= instrument.multiplierNeeded;

        return instrument.intensity;
    }
}