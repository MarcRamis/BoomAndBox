using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public SequenceController sequenceController; // Referencia al controlador de secuencias
    [SerializeField] public List<SequenceController> sequenceControllers; // Referencia que contiene todos los controladores de secuencias

    protected ButtonsSequence buttonsSequence; // Secuencia de botones actual
    protected bool rythmMoment; // Variable que indica si se está en un momento de ritmo
    protected bool simonIsPlaying = false; // Indica si el minijuego se está ejecutando

    public static SimonController instance;

    [Header("Settings")]
    [HideInInspector] protected bool rythmOpportunity = false; // Indica si hay oportunidad de presionar un botón en un momento de ritmo
    [SerializeField] protected float rythmOpportunityCd = 0.5f; // Tiempo mínimo entre oportunidades de presionar botón durante un momento de ritmo

    [SerializeField] protected MTimer rythmPlayerTimer; // Temporizador para la duración de los momentos de ritmo
    [SerializeField] protected float buttonPressedEndCd = 0.6f; // Tiempo mínimo entre el final de presionar un botón y el inicio del siguiente momento de ritmo
    [HideInInspector] protected bool buttonHasBeenPressed = false; // Indica si se ha presionado un botón
    [HideInInspector] protected bool correctButtonPressed = false; // Indica si se ha presionado el botón correcto
    
    private Combo freedCombo;

    // Eventos para cuando se presiona el botón correcto, se presiona el botón incorrecto, 
    // se presiona tarde un botón y no se presiona en un momento de ritmo
    public delegate void OnCorrectButtonEvent();
    public OnCorrectButtonEvent OnCorrectSequence;

    public delegate void OnWrongButtonEvent();
    public OnWrongButtonEvent OnCorrectButton;

    public delegate void OnTooLateButtonEvent();
    public OnTooLateButtonEvent OnTooLateButton;

    public delegate void OnNoRhythmPressedButtonEvent();
    public OnNoRhythmPressedButtonEvent OnNoRhythmPressedButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        // Crear y configurar un temporizador para el botón presionado
        rythmPlayerTimer = new MTimer(buttonPressedEndCd);
        rythmPlayerTimer.OnTimerEnd += ButtonPressedTimerEnd;
        
        freedCombo = new Combo();
        freedCombo.SetMaxCombo(5);
    }

    private void Start()
    {
        // Suscribir el método Rythm al evento OnBeat del objeto beat del RythmController
        RythmController.instance.beat.OnBeat += Rythm;
    }

    private void Update()
    {
        // Si Simon está jugando en el modo Simon Dice
        if (simonIsPlaying)
        {
            // Si hay una oportunidad de ritmo
            if (rythmOpportunity)
            {
                // Si YA se ha presionado un botón
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
            // Habilita la oportunidad de ritmo y comienza una cuenta regresiva para deshabilitarla
            rythmOpportunity = true;
            Invoke(nameof(ResetRythm), rythmOpportunityCd);
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
            // Inicia una cuenta regresiva para el temporizador del ritmo
            rythmPlayerTimer.StartTimer();
            buttonHasBeenPressed = true;
            controlType = ButtonPressed(buttonName);
            
            // Esta comprobación se hace para observar si el jugador está siguiendo una secuencia, en caso de que sea nula
            if (sequenceController.CheckIfFollowingASequence(controlType))
            {
                // Verifica si el botón presionado es el correcto
                if (CorrectButton(controlType))
                {
                    correctButtonPressed = true;      

                    // Actualiza la secuencia en el SequenceController
                    sequenceController.UpdateSequence();
                    
                    // Verifica si el jugador completó la secuencia actual
                    if (sequenceController.CheckIfPlayerFinished())
                    {
                        SumCombo(5);
                        OnCorrectSequence?.Invoke();
                    }
                }

                else
                {
                    correctButtonPressed = false;

                    SumCombo(1);
                    sequenceController.FollowingRandomRythm();
                }
            }
            else
            {
                sequenceController.FollowingRandomRythm();
            }
        }
    }
    
    private void SumCombo(int counter)
    {
        freedCombo.SumCombo(counter);
        if(freedCombo.ComboAccomplished())
        {
            sequenceController.Finish();
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
        freedCombo.Rest();
    }
    
    // Devuelve true si el tipo de control del jugador coincide con el tipo de control esperado en la secuencia
    private bool CorrectButton(EControlType eControlType)
    {
        if (sequenceController.GetCurrentSequence() != null)
        {
            if (sequenceController.GetCurrentControl() == eControlType)
            {
                OnCorrectButton?.Invoke();
                return true;
            }
        }

        return false;
    }

    // Inicializa el juego
    public void Initialize()
    {
        sequenceController.Init();
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
        if (simonIsPlaying)
        {
            // Verifica si se presionó el botón correcto y si ya ha pasado el tiempo permitido para presionar un botón, 
            // invoca el evento correspondiente y realiza la sincronización incorrecta
            if (correctButtonPressed)
            {
                OnTooLateButton?.Invoke();
            }
            WrongSyncronization();

            rythmPlayerTimer.StartTimer();
        }
    }

    protected void PlaySimon()
    {
        // Inicia la secuencia de Simon
        simonIsPlaying = true;
    }

    public void StopSimon()
    {
        // Detiene la secuencia de Simon y cambia al siguiente modo de juego
        simonIsPlaying = false;
    }

    public void SetSequenceController(int index)
    {
        sequenceController = sequenceControllers[index];
    }
}