using UnityEngine;

// Esta enumeración representa los controles disponibles en el controlador de PS4 (aunque podría ser lo mismo para Xbox)
public enum EControlType { NONE, SQUARE, CROSS, TRIANGLE, CIRCLE, UP, DOWN, RIGHT, LEFT }


// Esta clase representa una secuencia de botones, con un array de EControlType que representa los botones
// que deben ser pulsados en la secuencia correcta.
[System.Serializable]
public class ButtonsSequence
{
    public EControlType[] buttonSequence;
    public bool isCompleted;
    [HideInInspector] public int sequenceCounter;

    // Este campo se usa para mantener un seguimiento del botón actual en la secuencia
    [SerializeField] public EControlType currentLoopControl;

    // Este método se llama para avanzar al siguiente botón en la secuencia
    public void NextLoopControl()
    {
        currentLoopControl = buttonSequence[sequenceCounter];
    }
    
    public void SumLoopControl()
    {
        if (sequenceCounter + 1 < buttonSequence.Length)
        {
            sequenceCounter++;
            NextLoopControl();
        }
        else
        {
            isCompleted = true;
        }
    }

    // Este método se llama para establecer el botón inicial en la secuencia
    public void SetInitControl()
    {
        currentLoopControl = buttonSequence[0];
        sequenceCounter = 0;
    }

    public void Reset()
    {
        buttonSequence = null;
        isCompleted = false;
        currentLoopControl = EControlType.NONE;
    }
}
