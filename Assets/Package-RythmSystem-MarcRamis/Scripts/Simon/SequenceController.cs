using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SequenceController : MonoBehaviour
{
    // Definición de variables públicas y privadas
    [Header("References")]
    [SerializeField] protected List<ButtonsSequence> sequences; // Lista de secuencias que se reproducirán en esta canción
    [SerializeField] protected ButtonsSequence currentSequence; // La secuencia actual en la que se está trabajando

    // Método Init para inicializar el juego
    public void Init()
    {
        foreach (ButtonsSequence bs in sequences)
        {
            if (bs != sequences[0])
            {
                bs.isCompleted = false;
                bs.SetInitControl();
            }
        }
    }

    public void UpdateSequence()
    {
        FollowingSequence();
        FollowingRythm();
    }

    public void FollowingSequence()
    {
        // Pasamos al siguiente botón de la secuencia
        NextControl();
    }

    private void NextControl()
    {
        currentSequence.SumLoopControl();
    }

    public void Finish()
    {
        // Configuramos la fase final del soundtrackManager
        RythmController.instance.soundtrackManager.ConfigurateFinal();
    }
    
    public void FollowingRythm()
    {
        // Activamos el rythmo del soundtrackManager
        RythmController.instance.soundtrackManager.RythmOn();
    }
    
    public void FollowingRandomRythm()
    {
        RythmController.instance.soundtrackManager.RythmOnFreed();
        NotFollowingSequence();
    }

    public bool CheckIfPlayerFinished()
    {
        // Iteramos sobre la lista de secuencias
        foreach (ButtonsSequence sq in sequences)
        {
            // Si encontramos una secuencia completada, devolvemos true
            if (sq.isCompleted == true)
                return true;
        }

        return false;
    }
    
    // Método que se llama cuando el jugador ha presionado un botón incorrecto y la sincronización se pierde
    public void WrongSync()
    {
        RythmController.instance.soundtrackManager.RestSyncInstrument();
        NotFollowingSequence();
        Init();
    }
    
    public void NotFollowingSequence()
    {
        currentSequence = null;
        currentSequence = new ButtonsSequence();
    }

    // Método para obtener la secuencia actual
    public ButtonsSequence GetCurrentSequence() { return currentSequence; }
    
    public bool CheckIfFollowingASequence(EControlType eControlType)
    {
        if (currentSequence.currentLoopControl != EControlType.NONE)
            return true;
       
        else
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i] != sequences[0])
                {
                    // Quiero comprobar la primera vez que esté siguiendo un ritmo
                    // Si no sigue el ritmo, hay que devolver false
                    if (sequences[i].currentLoopControl == eControlType)
                    {
                        RythmController.instance.soundtrackManager.SelectConfiguration(i - 1);
                        currentSequence = sequences[i];
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Método para obtener el control actual de la secuencia
    public EControlType GetCurrentControl() { return currentSequence.currentLoopControl; }
}