using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected SequenceController sequenceController; // Referencia al controlador de secuencias
    
    protected ButtonsSequence buttonsSequence; // Secuencia de botones actual
    protected bool rythmMoment; // Variable que indica si se está en un momento de ritmo
    protected bool simonIsPlaying = false; // Indica si el minijuego se está ejecutando

    [Header("Settings")]
    [SerializeField] protected float timeToStartPlayerSimon = 5f; // Tiempo que tarda en empezar la secuencia del jugador después de que se haya mostrado el ejemplo
    [SerializeField] protected float timeToStartNextSequence = 5f; // Tiempo que tarda en empezar una nueva secuencia después de que se haya completado la actual

    [HideInInspector] protected bool rythmOpportunity = false; // Indica si hay oportunidad de presionar un botón en un momento de ritmo
    [SerializeField] protected float rythmOpportunityCd = 0.5f; // Tiempo mínimo entre oportunidades de presionar botón durante un momento de ritmo
    [SerializeField] protected ESimonMode mode = ESimonMode.EXAMPLE_SIMON; // Modo de juego de Simon (ejemplo o jugador)

    [SerializeField] protected MTimer rythmPlayerTimer; // Temporizador para la duración de los momentos de ritmo
    [SerializeField] protected float buttonPressedEndCd = 0.6f; // Tiempo mínimo entre el final de presionar un botón y el inicio del siguiente momento de ritmo
    [HideInInspector] protected bool buttonHasBeenPressed = false; // Indica si se ha presionado un botón
    [HideInInspector] protected bool correctButtonPressed = false; // Indica si se ha presionado el botón correcto

    // Eventos para cuando se presiona el botón correcto, se presiona el botón incorrecto, 
    // se completa la secuencia de ejemplo, se completa la secuencia del jugador, 
    // se presiona tarde un botón y no se presiona en un momento de ritmo
    public delegate void OnCorrectButtonEvent();
    public OnCorrectButtonEvent OnCorrectButton;

    public delegate void OnWrongButtonEvent();
    public OnWrongButtonEvent OnWrongButton;

    public delegate void OnExampleCompletedSequenceEvent();
    public OnExampleCompletedSequenceEvent OnExampleCompletedSequence;

    public delegate void OnPlayerCompletedSequenceEvent();
    public OnPlayerCompletedSequenceEvent OnPlayerCompletedSequence;

    public delegate void OnTooLateButtonEvent();
    public OnTooLateButtonEvent OnTooLateButton;

    public delegate void OnNoRhythmPressedButtonEvent();
    public OnNoRhythmPressedButtonEvent OnNoRhythmPressedButton;

    public delegate void OnStartEvent();
    public OnStartEvent OnStart;

    public delegate void OnFinishedEvent();
    public OnFinishedEvent OnFinished;

    private void Awake()
    {
        // Crear y configurar un temporizador para el botón presionado
        rythmPlayerTimer = new MTimer(buttonPressedEndCd);
        rythmPlayerTimer.OnTimerEnd += ButtonPressedTimerEnd;
    }

    private void Start()
    {
        // Suscribir el método Rythm al evento OnBeat del objeto beat del RythmController
        RythmController.instance.beat.OnBeat += Rythm;
    }


    private void Update()
    {
        // Si Simon está jugando en el modo Simon Dice
        if (simonIsPlaying && mode == ESimonMode.SIMONSAYS)
        {
            // Si hay una oportunidad de ritmo
            if (rythmOpportunity)
            {
                // Si se ha presionado un botón
                if (buttonHasBeenPressed)
                {
                    // Verificar si el jugador hizo una secuencia correcta sin ritmo
                    CheckNoRythm();
                }
                else
                {
                    // Verificar si el jugador hizo una secuencia correcta con ritmo
                    Check();
                }

            }
            else
            {
                // Verificar si el jugador hizo una secuencia correcta sin ritmo
                CheckNoRythm();
            }

            // Actualizar el temporizador para el botón presionado
            rythmPlayerTimer.Update(Time.deltaTime);
        }
    }

    private void Rythm()
    {
        // Verifica si Simon está jugando
        if (simonIsPlaying)
        {
            // Usa un switch statement para verificar el modo actual
            switch (mode)
            {
                case ESimonMode.EXAMPLE_SIMON:

                    // Actualiza la secuencia de ejemplo en el SequenceController
                    sequenceController.UpdateSequence(mode);

                    // Verifica si se terminó la secuencia de ejemplo
                    if (sequenceController.CheckIfExampleFinished())
                    {
                        // Detiene el juego de Simon y espera un tiempo antes de iniciar el juego del jugador
                        StopSimon();
                        Invoke(nameof(StartPlayerSimon), timeToStartPlayerSimon);
                    }

                    break;

                case ESimonMode.SIMONSAYS:

                    // Habilita la oportunidad de ritmo y comienza una cuenta regresiva para deshabilitarla
                    rythmOpportunity = true;
                    Invoke(nameof(ResetRythm), rythmOpportunityCd);

                    break;
            }
        }
    }

    public void Check()
    {
        // Verifica si los botones están presionados correctamente
        CheckButton("Cross");
        CheckButton("Circle");
        CheckButton("Square");
        CheckButton("Triangle");
    }

    private void CheckButton(string buttonName)
    {
        EControlType controlType = EControlType.NONE;

        // Verifica si el botón correspondiente se presionó
        if (Input.GetButtonDown(buttonName))
        {
            buttonHasBeenPressed = true;
            controlType = ButtonPressed(buttonName);

            // Verifica si el botón presionado es el correcto
            if (CorrectButton(controlType))
            {
                correctButtonPressed = true;

                // Inicia una cuenta regresiva para el temporizador del ritmo
                rythmPlayerTimer.StartTimer();

                // Actualiza la secuencia en el SequenceController
                sequenceController.UpdateSequence(mode);

                // Verifica si el jugador completó la secuencia actual
                if (sequenceController.CheckIfPlayerFinished())
                {
                    correctButtonPressed = false;
                    StopSimon();

                    // Verifica si hay una siguiente secuencia disponible
                    if (sequenceController.NextSequence())
                    {
                        // Espera un tiempo antes de iniciar la siguiente secuencia
                        Invoke(nameof(StartNextSequence), timeToStartNextSequence);
                    }
                    else
                    {
                        // Invoca el evento OnFinished si no hay más secuencias disponibles
                        OnFinished?.Invoke();
                        sequenceController.Finish();
                    }
                }
            }

            else
            {
                correctButtonPressed = false;

                // Invoca el evento OnWrongButton si el botón es incorrecto
                OnWrongButton?.Invoke();

                // Realiza la sincronización incorrecta
                WrongSyncronization();
            }
        }

    }


    // Devuelve el tipo de control correspondiente al nombre del botón
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

    // Método que se ejecuta cuando el jugador pulsa un botón incorrecto
    private void WrongSyncronization()
    {
        sequenceController.WrongSync();
    }

    // Devuelve true si el tipo de control del jugador coincide con el tipo de control esperado en la secuencia
    private bool CorrectButton(EControlType eControlType)
    {
        if (sequenceController.GetCurrentControl() == eControlType)
        {
            OnCorrectButton?.Invoke();
            return true;
        }
        return false;
    }

    // Inicia la reproducción de la secuencia de Simon para el jugador
    private void StartPlayerSimon()
    {
        OnExampleCompletedSequence?.Invoke();
        PlaySimon();
        sequenceController.NowSimonPlayer();
    }

    // Inicia la reproducción de la siguiente secuencia
    private void StartNextSequence()
    {
        OnPlayerCompletedSequence?.Invoke();
        PlaySimon();
    }

    // Inicializa el juego
    public void Initialize()
    {
        OnStart?.Invoke();
        PlaySimon();
    }

    public void CheckNoRythm()
    {
        // Verifica si se presiona alguno de los botones de la secuencia
        CheckNoButton("Cross");
        CheckNoButton("Circle");
        CheckNoButton("Square");
        CheckNoButton("Triangle");
    }

    private void CheckNoButton(string buttonName)
    {
        // Verifica si se presionó el botón indicado y si es así, invoca el evento correspondiente y realiza la sincronización incorrecta
        if (Input.GetButtonDown(buttonName))
        {
            OnNoRhythmPressedButton?.Invoke();
            WrongSyncronization();
        }
    }

    private void ResetRythm()
    {
        // Reinicia las variables relacionadas con el ritmo
        rythmOpportunity = false;
        buttonHasBeenPressed = false;
    }

    private void ButtonPressedTimerEnd()
    {
        // Verifica si se presionó el botón correcto y si ya ha pasado el tiempo permitido para presionar un botón, invoca el evento correspondiente y realiza la sincronización incorrecta
        if (correctButtonPressed)
        {
            OnTooLateButton?.Invoke();
        }
        WrongSyncronization();
    }

    protected void PlaySimon()
    {
        // Inicia la secuencia de Simon
        simonIsPlaying = true;
    }

    protected void StopSimon()
    {
        // Detiene la secuencia de Simon y cambia al siguiente modo de juego
        simonIsPlaying = false;
        SwapMode();
    }

    private void SwapMode()
    {
        // Cambia el modo de juego entre "Ejemplo de Simon" y "Simon dice"
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