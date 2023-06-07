using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ESimonMode { EXAMPLE_SIMON, SIMONSAYS }

public class SequenceController : MonoBehaviour
{
    // Definición de variables públicas y privadas
    [Header("References")]
    [SerializeField] protected List<ButtonsSequence> sequences; // Lista de secuencias que se reproducirán en esta canción
    [HideInInspector] protected ButtonsSequence currentSequence; // La secuencia actual en la que se está trabajando

    [SerializeField] private GameObject exampleSequenceContainer; // Un contenedor para llenar y donde se almacena el ejemplo del jugador de la secuencia
    [SerializeField] private GameObject sequenceContainer; // Un contenedor para llenar, con el que el jugador debe sincronizar
    [HideInInspector] public List<GameObject> currentExampleSequenceGO; // Lista de GameObjects del ejemplo de la secuencia actual
    [HideInInspector] public List<GameObject> currentSequenceGO; // Lista de GameObjects de la secuencia actual
    [HideInInspector] public GameObject currentControlToShow; // Control actual para mostrar en pantalla

    [Space]
    [Header("Buttons Image")]
    [SerializeField] private GameObject square; // Imagen para el botón cuadrado
    [SerializeField] private GameObject cross; // Imagen para el botón cruz
    [SerializeField] private GameObject triangle; // Imagen para el botón triángulo
    [SerializeField] private GameObject circle; // Imagen para el botón círculo

    [HideInInspector] private GameObject up; // Imagen para el botón arriba
    [HideInInspector] private GameObject down; // Imagen para el botón abajo
    [HideInInspector] private GameObject right; // Imagen para el botón derecha
    [HideInInspector] private GameObject left; // Imagen para el botón izquierda

    // Método Awake, se ejecuta al inicio del script
    public void Awake()
    {
        Init();
    }

    // Método Init para inicializar el juego
    public void Init()
    {
        currentSequence = sequences[1]; // Asignar la segunda secuencia a la variable currentSequence
        EControlType[] newSequence = currentSequence.buttonSequence; // Obtener la secuencia de botones de la secuencia actual
        CreateSequence(newSequence); // Crear la secuencia de botones a partir de la secuencia actual
    }

    public void CreateSequence(EControlType[] controlType)
    {
        ClearSequence(); // Limpiar cualquier secuencia previa en el contenedor

        // Iterar sobre cada control en la secuencia y agregarlos al contenedor de secuencia de ejemplo y de la secuencia actual
        foreach (EControlType control in controlType)
        {
            HandleControlType(control);
        }

        // Establecer el control actual que se muestra en el primer control de la secuencia de ejemplo
        currentControlToShow = currentExampleSequenceGO[0];

        // Establecer el control inicial en la secuencia actual
        currentSequence.SetInitControl();
    }

    private void HandleControlType(EControlType control)
    {
        // Agregar el objeto correspondiente al control en el contenedor de la secuencia de ejemplo y de la secuencia actual
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
        // Destruir todos los hijos de los contenedores de la secuencia de ejemplo y de la secuencia actual
        foreach (Transform child in exampleSequenceContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in sequenceContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Limpiar las listas de objetos de la secuencia de ejemplo y de la secuencia actual
        currentExampleSequenceGO.Clear();
        currentSequenceGO.Clear();
    }

    private void AddControl(GameObject button)
    {
        // Instanciamos un nuevo botón de secuencia en el contenedor de secuencias
        GameObject newButtonSequence = Instantiate(button, sequenceContainer.transform);
        // Lo desactivamos para que no se muestre por defecto
        newButtonSequence.SetActive(false);
        // Agregamos el nuevo botón a la lista de la secuencia actual
        currentSequenceGO.Add(newButtonSequence);

        // Instanciamos un nuevo botón de ejemplo de secuencia en el contenedor de ejemplo de secuencias
        GameObject newButtonExampleSequence = Instantiate(button, exampleSequenceContainer.transform);
        // Lo desactivamos para que no se muestre por defecto
        newButtonExampleSequence.SetActive(false);
        // Agregamos el nuevo botón a la lista de ejemplo de la secuencia actual
        currentExampleSequenceGO.Add(newButtonExampleSequence);
    }

    public void UpdateSequence(ESimonMode eSimonMode)
    {
        switch (eSimonMode)
        {
            case ESimonMode.EXAMPLE_SIMON:
                // Si estamos en modo de ejemplo de Simon, mostramos la secuencia de ejemplo
                ShowOnRythm(currentExampleSequenceGO);
                break;

            case ESimonMode.SIMONSAYS:
                // Si estamos en modo "Simon dice", mostramos la secuencia y esperamos a que el jugador la siga
                ShowOnRythm(currentSequenceGO);
                FollowingRythm();
                break;
        }
    }

    public void ShowOnRythm(List<GameObject> sequence)
    {
        // Mostramos el primer botón de la secuencia actual
        currentControlToShow.SetActive(true);
        // Pasamos al siguiente botón de la secuencia
        NextControl(sequence);
    }


    private void NextControl(List<GameObject> sequence)
    {
        // Iteramos sobre la lista de botones en la secuencia
        for (int i = 0; i < sequence.ToArray().Length - 1; i++)
        {
            // Comprobamos si el botón actual es el que estamos mostrando
            if (sequence[i] == currentControlToShow)
            {
                // Si el siguiente botón en la secuencia no es nulo,
                // actualizamos el botón a mostrar y avanzamos el loop
                if (sequence[i + 1] != null)
                {
                    currentControlToShow = sequence[i + 1];
                    currentSequence.NextLoopControl(i + 1);
                    break;
                }
                // Si el siguiente botón en la secuencia es nulo, salimos del loop
                break;
            }
        }
    }

    public bool NextSequence()
    {
        // Iteramos sobre la lista de secuencias
        for (int i = 0; i < sequences.Count - 1; i++)
        {
            // Comprobamos si la secuencia actual es la que estamos mostrando
            if (sequences[i] == currentSequence)
            {
                // Si la siguiente secuencia no es nula,
                // actualizamos la secuencia actual y creamos una nueva secuencia
                if (sequences[i + 1] != null)
                {
                    currentSequence = sequences[i + 1];
                    EControlType[] newSequence = currentSequence.buttonSequence;
                    CreateSequence(newSequence);

                    // Sumamos la configuración al soundtrackManager
                    RythmController.instance.soundtrackManager.SumConfiguration();

                    return true;
                }
                // Si la siguiente secuencia es nula, salimos del loop
                break;
            }
        }
        return false;
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

    public void NotFollowingRythm()
    {
        // Desactivamos el rythmo del soundtrackManager
        RythmController.instance.soundtrackManager.RythmOff();
    }

    public bool CheckIfLoopFinished(List<GameObject> sequence)
    {
        // Iteramos sobre la lista de botones en la secuencia
        foreach (GameObject sq in sequence)
        {
            // Si encontramos un botón que no esté activo, devolvemos false
            if (sq.activeSelf == false)
                return false;
        }

        // Si todos los botones están activos, devolvemos true
        return true;
    }

    public bool CheckIfExampleFinished()
    {
        // Iteramos sobre la lista de botones en la secuencia de ejemplo
        foreach (GameObject sq in currentExampleSequenceGO)
        {
            // Si encontramos un botón que no esté activo, devolvemos false
            if (sq.activeSelf == false)
                return false;
        }

        // Si todos los botones están activos, devolvemos true
        return true;
    }


    // Método que verifica si el jugador ha completado toda la secuencia actual
    public bool CheckIfPlayerFinished()
    {
        foreach (GameObject sq in currentSequenceGO)
        {
            if (sq.activeSelf == false)
                return false;
        }
        return true;
    }

    // Método que se llama cuando el jugador ha presionado un botón incorrecto y la sincronización se pierde
    public void WrongSync()
    {
        foreach (GameObject sq in currentSequenceGO)
        {
            sq.SetActive(false);
        }
        // Reiniciar la secuencia del jugador
        RestartPlayerSequence();
    }

    // Método que se llama cuando es el turno de Simon para mostrar la secuencia
    public void NowSimonPlayer()
    {
        // Reiniciar la secuencia del jugador
        RestartPlayerSequence();
    }

    // Método privado para reiniciar la secuencia del jugador
    private void RestartPlayerSequence()
    {
        // Reiniciar el control actual
        currentSequence.SetInitControl();
        currentControlToShow = currentSequenceGO[0];

        // Detener la sincronización con la música
        NotFollowingRythm();
    }

    // Método para obtener la secuencia del jugador
    public List<GameObject> GetPlayerSequence() { return currentSequenceGO; }

    // Método para obtener la secuencia de ejemplo actual
    public List<GameObject> GetExampleSequence() { return currentExampleSequenceGO; }

    // Método para obtener la secuencia actual
    public ButtonsSequence GetCurrentSequence() { return currentSequence; }

    // Método para obtener el control actual de la secuencia
    public EControlType GetCurrentControl() { return currentSequence.currentLoopControl; }

}