using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFC : FeedbackController
{
    
    [SerializeField] private AudioClip expSnd;

    private void Start()
    {
        PlaySoundEffect(expSnd, 0.5f);
    }
}
