using System;
using UnityEngine;

// this controls corresponds to the ps4 controller but xbox could be the same
public enum EControlType { NONE, SQUARE, CROSS, TRIANGLE, CIRCLE, UP, DOWN, RIGHT, LEFT }

[System.Serializable]
public struct ControlInput
{
    public EControlType control;
}

[System.Serializable]
public class ButtonsSequence
{
    public EControlType[] buttonSequence;
    
    //public delegate void SequenceCompletedEvent();
    //public event SequenceCompletedEvent OnSequenceCompleted;

    [HideInInspector] public EControlType currentLoopControl;

    public void NextLoopControl(int index)
    {
        currentLoopControl = buttonSequence[index];
    }
   
    public void SetInitControl()
    {
        currentLoopControl = buttonSequence[0];
    }
}
