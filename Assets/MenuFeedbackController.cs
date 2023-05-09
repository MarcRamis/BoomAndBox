using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFeedbackController : FeedbackController
{
    [SerializeField] private AudioClip buttonPresetSnd;

    public void PlayPressedButton()
    {
        PlaySoundEffect(buttonPresetSnd, 1.0f, 1.0f, 0.9f, 1.1f);
    }


}
