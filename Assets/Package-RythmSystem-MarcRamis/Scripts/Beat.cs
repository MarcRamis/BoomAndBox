using UnityEngine;

// Define una clase serializable llamada "Beat"
[System.Serializable]
public class Beat
{
    // Delegado "BeatEvent" y evento "OnBeat" utilizados para notificar cuando se detecta un beat
    public delegate void BeatEvent();
    public event BeatEvent OnBeat;

    // Booleano que determina si se puede detectar un nuevo beat
    public bool canRythm = false;

    // Cantidad de tiempo que debe transcurrir antes de que se pueda detectar otro beat
    public float cd = 0.5f;

    // Temporizador utilizado para determinar cuándo se puede detectar un nuevo beat
    MTimer rythmTimer;

    // Constructor de la clase
    public Beat()
    {
        // Inicializa el temporizador con el valor de "cd"
        rythmTimer = new MTimer(cd);

        // Asigna un manejador de eventos "OnTimerEnd" al temporizador
        rythmTimer.OnTimerEnd += ResetRythm;
    }

    // Método utilizado para actualizar el estado del objeto "Beat" cada vez que se actualiza el marco del juego
    public void Update(bool isBeat)
    {
        // Si se detecta un beat y no se ha detectado otro previamente
        if (isBeat && !canRythm)
        {
            // Inicia el temporizador
            rythmTimer.StartTimer();

            // Invoca el evento "OnBeat"
            OnBeat?.Invoke();

            // Indica que no se puede detectar otro beat hasta que el temporizador alcance su límite de tiempo
            canRythm = true;
        }

        // Actualiza el temporizador
        rythmTimer.Update(Time.deltaTime);
    }

    // Método utilizado para restablecer la variable "canRythm" a false cuando el temporizador alcanza su límite de tiempo
    private void ResetRythm()
    {
        canRythm = false;
    }
}
