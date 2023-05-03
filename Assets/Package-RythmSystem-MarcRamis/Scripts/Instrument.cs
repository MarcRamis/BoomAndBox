using UnityEngine;

[System.Serializable]
public class Instrument
{
    public AudioSource instrumentRef;

    public float threshold;
    public float multiplierNeeded;
    public float intensity;
    
    public bool canRythm = false;
    public bool rythmOnce = true;
    public float rythmOpportunityCd = 0.5f;

    private MTimer rythmMomentTimer;

    public bool IsRythmMoment() { return (intensity * instrumentRef.volume) > (threshold * instrumentRef.volume);  }

}
