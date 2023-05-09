using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode openMenu = KeyCode.Escape;

    [Header("Canvas")]
    [SerializeField] private Canvas canvas = null;

    //[Header("EventSystem")]
    //[SerializeField] private EventsSystem eventsSystem = null;

    [Header("InputController")]
    [SerializeField] private MenuInputController inputsUI;
    [HideInInspector] private Player player;

    [Header("Pause-Menu")]
    [SerializeField] private GameObject options;

    [Header("Buttons")]
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject sliderMaster;

    private bool isMenuOpen = false;

    private void Awake()
    {
        inputsUI.onPauseInput += TogglePauseMenuState;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            //if(eventsSystem == null)
            //    eventsSystem = FindObjectOfType<EventsSystem>();
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Options") && isMenuOpen)
        {
            Debug.Log("start");
        }
    }

    public void TogglePauseMenuState()
    {
        if (isMenuOpen)
        {
            if (player != null)
                player.AllowInputs();

            canvas.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        else
        {
            if (player != null)
                player.BlockInputs();
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(resumeButton, new BaseEventData(eventSystem));
            canvas.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        isMenuOpen = !isMenuOpen;
    }
    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Options()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(sliderMaster, new BaseEventData(eventSystem));
        options.SetActive(true);
    }

    public void CloseOptions()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(resumeButton, new BaseEventData(eventSystem));
        options.SetActive(false);
    }

    private void OnDestroy()
    {
        inputsUI.onPauseInput -= TogglePauseMenuState;
    }
}
