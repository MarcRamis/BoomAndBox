using System;

[System.Serializable]

public class ButtonsSequence
{
    public EControlType[] buttonSequence;
    public bool isCompleted;
    
    public EControlType currentLoopControl;

    public Instrument instrument; // the instrument properties to set up the song rythm moment

    public void NextLoopControl()
    {
        for (int i = 0; i < buttonSequence.Length - 1; i++)
        {
            if (buttonSequence[i + 1] != null)
            {
                currentLoopControl = buttonSequence[i + 1];
                break;
            }
        }
    }

    public void NextLoopControl(int index)
    {
        currentLoopControl = buttonSequence[index];
    }

    public void SetInitControl()
    {
        currentLoopControl = buttonSequence[0];
    }
}
