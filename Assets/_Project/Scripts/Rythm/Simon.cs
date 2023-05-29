using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Simon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ThrowingSystem throwingSystem;
    [SerializeField] private DecalProjector graffiti;
    [SerializeField] private CustomSimonEvent simonEvent;
    
    [Header("Targets")]
    [Space]
    [SerializeField] private Transform[] graffitiTargets;
    [HideInInspector] private Transform currentTarget;
    
    private void Start()
    {
        SimonController.instance.OnCorrectButton += CorrectButton;
        SimonController.instance.OnCorrectSequence += CorrectSequence;
        SimonController.instance.OnTooLateButton += TooLateButton;
        SimonController.instance.OnNoRhythmPressedButton += NoRhythmPressedButton;
        
        RythmController.instance.OnFreeMode += FreeMode;
        RythmController.instance.OnScheduledMode += ScheduledMode;
    }
    
    private void CorrectButton()
    {
        //currentTarget = MathUtils.Randomize.GetRandomTarget(graffitiTargets);
        //throwingSystem.ThrowLarge(currentTarget.position, 50f);
    }
    private void CorrectSequence()
    {

    }
    private void TooLateButton()
    {

    }
    private void NoRhythmPressedButton()
    {

    }

    private void FreeMode()
    {

    }
    private void ScheduledMode()
    {

    }
}