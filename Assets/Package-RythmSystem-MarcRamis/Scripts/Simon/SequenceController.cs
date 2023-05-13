using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ESimonMode { EXAMPLE_SIMON, SIMONSAYS }

public class SequenceController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected List<ButtonsSequence> sequences; // all the sequence that will be played in this song
    [HideInInspector] protected ButtonsSequence currentSequence; // my current sequence working on 
    
    [SerializeField] private GameObject exampleSequenceContainer; // a container to fill up and where are storeds they player example of the sequence
    [SerializeField] private GameObject sequenceContainer;  // a container to fill up the player must sync with
    [HideInInspector] public List<GameObject> currentExampleSequenceGO; 
    [HideInInspector] public List<GameObject> currentSequenceGO;
    [HideInInspector] public GameObject currentControlToShow;
    
    [Space]
    [Header("Buttons Image")]
    [SerializeField] private GameObject square;
    [SerializeField] private GameObject cross;
    [SerializeField] private GameObject triangle;
    [SerializeField] private GameObject circle;
    
    [HideInInspector] private GameObject up;
    [HideInInspector] private GameObject down;
    [HideInInspector] private GameObject right;
    [HideInInspector] private GameObject left;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        currentSequence = sequences[1];
        EControlType[] newSequence = currentSequence.buttonSequence;
        CreateSequence(newSequence);
    }


    public void CreateSequence(EControlType[] controlType)
    {
        ClearSequence();

        foreach (EControlType control in controlType)
        {
            HandleControlType(control);
        }
        currentControlToShow = currentExampleSequenceGO[0];
        currentSequence.SetInitControl();
    }

    private void HandleControlType(EControlType control)
    {
        switch (control)
        {
            case EControlType.SQUARE:

                AddControl(square.gameObject);

                break;
            case EControlType.CROSS:

                AddControl(cross.gameObject);

                break;
            case EControlType.TRIANGLE:

                AddControl(triangle.gameObject);

                break;
            case EControlType.CIRCLE:

                AddControl(circle.gameObject);

                break;
            case EControlType.UP:
                break;
            case EControlType.DOWN:
                break;
            case EControlType.RIGHT:
                break;
            case EControlType.LEFT:
                break;
        }
    }

    private void ClearSequence()
    {
        foreach (Transform child in exampleSequenceContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in sequenceContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        currentExampleSequenceGO.Clear();
        currentSequenceGO.Clear();
    }
    
    private void AddControl(GameObject button)
    {
        GameObject newButtonSequence = Instantiate(button, sequenceContainer.transform);
        newButtonSequence.SetActive(false);
        currentSequenceGO.Add(newButtonSequence);

        GameObject newButtonExampleSequence = Instantiate(button, exampleSequenceContainer.transform);
        newButtonExampleSequence.SetActive(false);
        currentExampleSequenceGO.Add(newButtonExampleSequence);
    }
    
    public void UpdateSequence(ESimonMode eSimonMode)
    {
        switch (eSimonMode)
        {
            case ESimonMode.EXAMPLE_SIMON:

                ShowOnRythm(currentExampleSequenceGO);

                break;
                
            case ESimonMode.SIMONSAYS:
                
                ShowOnRythm(currentSequenceGO);
                FollowingRythm();

                break;
        }
    }

    public void ShowOnRythm(List<GameObject> sequence)
    { 
        currentControlToShow.SetActive(true);
        NextControl(sequence);
    }
    
    private void NextControl(List<GameObject> sequence)
    {
        for(int i = 0; i < sequence.ToArray().Length - 1; i++)
        {
            if (sequence[i] == currentControlToShow)
            {
                if (sequence[i + 1] != null)
                {
                    currentControlToShow = sequence[i + 1];
                    currentSequence.NextLoopControl(i + 1);
                    break;
                }
                break;
            }
        }
    }

    public bool NextSequence()
    {
        for (int i = 0; i < sequences.Count - 1; i++)
        {
            if (sequences[i] == currentSequence)
            {
                if (sequences[i + 1] != null)
                {
                    currentSequence = sequences[i + 1];
                    EControlType[] newSequence = currentSequence.buttonSequence;
                    CreateSequence(newSequence);
                    
                    RythmController.instance.soundtrackManager.SumConfiguration();

                    return true;
                }
                break;
            }
        }
        return false;
    }

    public void Finish()
    {
        RythmController.instance.soundtrackManager.ConfigurateFinal();
    }

    public void FollowingRythm()
    {
        RythmController.instance.soundtrackManager.RythmOn();
    }

    public void NotFollowingRythm()
    {
        RythmController.instance.soundtrackManager.RythmOff();
    }

    public bool CheckIfLoopFinished(List<GameObject> sequence)
    {
        foreach(GameObject sq in sequence)
        {
            if (sq.activeSelf == false)
                return false;
        }

        return true;
    }

    public bool CheckIfExampleFinished()
    {
        foreach (GameObject sq in currentExampleSequenceGO)
        {
            if (sq.activeSelf == false)
                return false;
        }

        return true;
    }

    public bool CheckIfPlayerFinished()
    {
        foreach (GameObject sq in currentSequenceGO)
        {
            if (sq.activeSelf == false)
                return false;
        }

        return true;
    }

    
    public void WrongSync()
    {
        foreach (GameObject sq in currentSequenceGO)
        {
            sq.SetActive(false);
        }
        
        RestartPlayerSequence();
    }

    public void NowSimonPlayer()
    {
        RestartPlayerSequence();
    }

    private void RestartPlayerSequence()
    {
        currentSequence.SetInitControl();
        currentControlToShow = currentSequenceGO[0];
        NotFollowingRythm();
    }

    public List<GameObject> GetPlayerSequence() { return currentSequenceGO; }
    public List<GameObject> GetExampleSequence() { return currentExampleSequenceGO; }
    public ButtonsSequence GetCurrentSequence() { return currentSequence; }
    public EControlType GetCurrentControl() { return currentSequence.currentLoopControl; }
}