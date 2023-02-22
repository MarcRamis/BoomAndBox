using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    // Referencia al componente AudioSource
    [Header("Sound Effects")]
    public AudioSource audioSource;

    [System.Serializable]
    public class AudioFeedback
    {
        public AudioClip clip;
        public int priority;
    }
    // Lista de clips de audio y sus prioridades
    public List<AudioFeedback> audioFeedbacks;

    // The 'virtual' modifier is used to allow a method to be overridden in derived classes. 
    // When a method is declared as 'virtual', it means that it can be redefined in a derived class 
    // using the 'override' keyword. If a derived class does not provide an implementation for 
    // the virtual method, the base class implementation is used.
    public virtual void PlaySoundEffect(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    // Función para reproducir un sonido con prioridad
    public virtual void PlaySoundWithPriority(AudioClip sound)
    {
        // Buscar el clip de audio con la mayor prioridad
        AudioFeedback highestPriorityFeedback = null;
        foreach (AudioFeedback audioFeedback in audioFeedbacks)
        {
            if (audioFeedback.clip == sound)
            {
                if (highestPriorityFeedback == null || audioFeedback.priority > highestPriorityFeedback.priority)
                {
                    highestPriorityFeedback = audioFeedback;
                }
            }
        }

        // Reproducir el clip de audio con la mayor prioridad
        if (highestPriorityFeedback != null)
        {
            audioSource.PlayOneShot(highestPriorityFeedback.clip);
        }
    }
}

public class AudioController
{ 
}