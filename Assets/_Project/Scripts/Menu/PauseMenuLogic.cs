using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PauseMenuLogic : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode openMenu = KeyCode.Escape;

    [Header("Canvas")]
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private Slider slider = null;

    [Header("InputController")]
    [SerializeField] private MenuInputController inputsUI;
    [SerializeField] private Player player;

    [Header("Pause-Menu")]
    [SerializeField] private GameObject options;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Buttons")]
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject sliderMaster;

    private bool isMenuOpen = false;
    private float sound = 0.0f;

    private void Awake()
    {
        inputsUI.onPauseInput += TogglePauseMenuState;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        float value;
        audioMixer.GetFloat("MasterVolume", out value);
        sound = value;
        slider.value = DecibelToLinear(sound);
        //MasterMusicControl(sound);
        //Debug.Log(DecibelToLinear(sound));

    }

    private void Start()
    {
        
    }

    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);
        return linear;
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

    public void MasterMusicControl(float _sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(_sliderValue) * 20.0f);
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
