using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Simon : MonoBehaviour
{
    [SerializeField] private ThrowingSystem throwingSystem;
    [SerializeField] private SimonController simonController;
    [SerializeField] private DecalProjector graffiti;
    [SerializeField] private CustomSimonEvent simonEvent;

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