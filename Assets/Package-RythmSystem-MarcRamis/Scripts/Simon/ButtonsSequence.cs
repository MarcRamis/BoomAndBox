using UnityEngine;

// Esta enumeración representa los controles disponibles en el controlador de PS4 (aunque podría ser lo mismo para Xbox)
public enum EControlType { NONE, SQUARE, CROSS, TRIANGLE, CIRCLE, UP, DOWN, RIGHT, LEFT }

// Esta estructura representa un único input en el controlador, con un campo para el tipo de control
[System.Serializable]
public struct ControlInput
{
    public EControlType control;
}

// Esta clase representa una secuencia de botones, con un array de EControlType que representa los botones
// que deben ser pulsados en la secuencia correcta.
[System.Serializable]
public class ButtonsSequence
{
    public EControlType[] buttonSequence;

    // Este campo se usa para mantener un seguimiento del botón actual en la secuencia
    [HideInInspector] public EControlType currentLoopControl;

    // Este método se llama para avanzar al siguiente botón en la secuencia
    public void NextLoopControl(int index)
    {
        currentLoopControl = buttonSequence[index];
    }

    // Este método se llama para establecer el botón inicial en la secuencia
    public void SetInitControl()
    {
        currentLoopControl = buttonSequence[0];
    }
}
