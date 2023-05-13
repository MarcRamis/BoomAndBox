using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected SequenceController sequenceController;
    
    // DIFFERENT INSTRUMENTS TEST
    [HideInInspector] protected Image imgTest;
    [HideInInspector] protected Image imgTest2;
    [HideInInspector] protected Image imgTest3;
    [HideInInspector] protected Image imgTest4;
    
    protected bool rythmTest1;
    protected bool rythmTest2;
    protected bool rythmTest3;
    protected bool rythmTest4;
    //
    
    protected ButtonsSequence buttonsSequence;
    protected bool rythmMoment;
    protected bool simonIsPlaying = false;
    
    [Header("Settings")]
    [SerializeField] protected float timeToStartPlayerSimon = 5f; // the time to start the player sequence after the example has been shown
    [SerializeField] protected float timeToStartNextSequence = 5f; // the time to start a new sequence after the current is finished

    [HideInInspector] protected bool rythmOpportunity = false;
    [SerializeField] protected float rythmOpportunityCd = 0.5f;
    [SerializeField] protected ESimonMode mode = ESimonMode.EXAMPLE_SIMON;
    
    // Button pressed variables
    [SerializeField] protected MTimer rythmPlayerTimer;
    [SerializeField] protected float buttonPressedEndCd = 0.6f; // using this cooldown because if the player hasn't pressed any button in the rythm sequence between this constant, the sequence must reset
    [HideInInspector] protected bool buttonHasBeenPressed = false; // button to avoid the player could make a second click super fast

    private void Awake()
    {
        RythmController.instance.beat.OnBeat += Rythm;

        PlaySimon();
        rythmPlayerTimer = new MTimer(buttonPressedEndCd);
        rythmPlayerTimer.OnTimerEnd += ButtonPressedTimerEnd;
    }

    
    private void Update()
    {
        if (simonIsPlaying && mode == ESimonMode.SIMONSAYS)
        {
            if (rythmOpportunity)
            {
                if (buttonHasBeenPressed)
                {
                    CheckNoRythm();
                }
                else
                {
                    Check();
                }

            }
            else
            {
                CheckNoRythm();
            }
            
            rythmPlayerTimer.Update(Time.deltaTime);
        }
    }
    
    
    private void Rythm()
    {
        if (simonIsPlaying)
        {
            switch(mode)
            {
                case ESimonMode.EXAMPLE_SIMON:

                    sequenceController.UpdateSequence(mode);

                    if (sequenceController.CheckIfExampleFinished())
                    {
                        StopSimon();
                        Invoke(nameof(StartPlayerSimon), timeToStartPlayerSimon);
                    }

                    break;

                case ESimonMode.SIMONSAYS:

                    rythmOpportunity = true;
                    Invoke(nameof(ResetRythm), rythmOpportunityCd);

                    break;
            }

        }
    }

    public void Check()
    {
        CheckButton("Cross");
        CheckButton("Circle");
        CheckButton("Square");
        CheckButton("Triangle");
    }

    private void CheckButton(string buttonName)
    {
        EControlType controlType = EControlType.NONE;
        
        if (Input.GetButtonDown(buttonName))
        {
            buttonHasBeenPressed = true;
            rythmPlayerTimer.StartTimer();
            controlType = ButtonPressed(buttonName);
            
            if (CorrectButton(controlType))
            {
                sequenceController.UpdateSequence(mode);
                
                if (sequenceController.CheckIfPlayerFinished())
                {
                    StopSimon();
                    if (sequenceController.NextSequence())
                    {
                        Invoke(nameof(StartNextSequence), timeToStartNextSequence);
                    }
                    else
                    {
                        sequenceController.Finish();
                    }
                }

            }
            else
            {
                sequenceController.WrongSync();
            }
        }

    }

    private static EControlType ButtonPressed(string buttonName)
    {
        EControlType controlType = EControlType.NONE;
        switch (buttonName)
        {
            case "Cross":
                controlType = EControlType.CROSS;
                break;
            case "Circle":
                controlType = EControlType.CIRCLE;
                break;
            case "Square":
                controlType = EControlType.SQUARE;
                break;
            case "Triangle":
                controlType = EControlType.TRIANGLE;
                break;
            default:
                Debug.Log("button name not implemented");
                break;
        }
        return controlType;
    }


    private bool CorrectButton(EControlType eControlType)
    {
        if (sequenceController.GetCurrentControl() == eControlType)
        {
            return true;
        }
        return false;
    }
    
    private void StartPlayerSimon()
    {
        Debug.Log("simon for player starts");
        PlaySimon();
        sequenceController.NowSimonPlayer();
    }

    private void StartNextSequence()
    {
        Debug.Log("nextSequence");
        PlaySimon();
    }


    public void CheckNoRythm()
    {
        CheckNoButton("Cross");
        CheckNoButton("Circle");
        CheckNoButton("Square");
        CheckNoButton("Triangle");
    }

    private void CheckNoButton(string buttonName)
    {
        if (Input.GetButtonDown(buttonName))
        {
            sequenceController.WrongSync();
        }
    }

    private void ResetRythm()
    {
        rythmOpportunity = false;
        buttonHasBeenPressed = false;
    }

    private void ButtonPressedTimerEnd()
    {
        sequenceController.WrongSync();
    }

    private void Test()
    {
        rythmTest1 = RythmController.instance.soundtrackManager.GetAllInstruments()[2].IsIntensityGreater();
        rythmTest2 = RythmController.instance.soundtrackManager.GetAllInstruments()[3].IsIntensityGreater();
        rythmTest3 = RythmController.instance.soundtrackManager.GetAllInstruments()[4].IsIntensityGreater();
        rythmTest4 = RythmController.instance.soundtrackManager.GetAllInstruments()[5].IsIntensityGreater();

        if (rythmTest1)
        {
            imgTest.color = Color.black;
            Invoke(nameof(ResetRythm1), 0.2f);
        }
        if (rythmTest2)
        {
            imgTest2.color = Color.red;
            Invoke(nameof(ResetRythm2), 0.2f);
        }
        if (rythmTest3)
        {
            imgTest3.color = Color.green;
            Invoke(nameof(ResetRythm3), 0.2f);
        }
        if (rythmTest4)
        {
            imgTest4.color = Color.cyan;
            Invoke(nameof(ResetRythm4), 0.2f);
        }
    }
    
    private void ResetRythm1()
    {
        imgTest.color = Color.white;
    }

    private void ResetRythm2()
    {
        imgTest2.color = Color.white;
    }

    private void ResetRythm3()
    {
        imgTest3.color = Color.white;
    }
    
    private void ResetRythm4()
    {
        imgTest4.color = Color.white;
    }

    protected void PlaySimon()
    {
        simonIsPlaying = true;
    }
    protected void StopSimon()
    {
        simonIsPlaying = false;
        SwapMode();
    }

    private void SwapMode()
    {
        switch (mode)
        {
            case ESimonMode.EXAMPLE_SIMON:

                mode = ESimonMode.SIMONSAYS;

                break;
            case ESimonMode.SIMONSAYS:

                mode = ESimonMode.EXAMPLE_SIMON;

                break;
            default:
                break;
        }
    }
}