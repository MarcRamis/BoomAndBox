using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Simon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ThrowingSystem throwingSystem;
    [SerializeField] private SimonController simonController;
    [SerializeField] private DecalProjector graffiti;
    [SerializeField] private CustomSimonEvent simonEvent;
    
    [Header("Targets")]
    [Space]
    [SerializeField] private Transform[] graffitiTargets;
    [HideInInspector] private Transform currentTarget;

    private void Start()
    {
        simonEvent.OnTrigger += simonController.Initialize;
        
        simonController.OnCorrectButton += CorrectButton;
        simonController.OnWrongButton += WrongButton;
        simonController.OnTooLateButton += TooLateButton;
        simonController.OnNoRhythmPressedButton += NoRhythmPressedButton;
        simonController.OnExampleCompletedSequence += ExampleCompletedSequence;
        simonController.OnPlayerCompletedSequence += PlayerCompletedSequence;
        simonController.OnStart += StartSimon;
        simonController.OnFinished += Finished;
    }
    
    private void CorrectButton()
    {
        currentTarget = MathUtils.Randomize.GetRandomTarget(graffitiTargets);
        throwingSystem.ThrowLarge(currentTarget.position, 50f);
    }
    private void WrongButton()
    {

    }
    private void TooLateButton()
    {

    }
    private void NoRhythmPressedButton()
    {

    }
    private void ExampleCompletedSequence()
    {

    }
    private void PlayerCompletedSequence()
    {

    }
    private void StartSimon()
    {

    }
    private void Finished()
    {

    }
}