using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode openMenu = KeyCode.Escape;

    [Header("Canvas")]
    [SerializeField] private Canvas canvas = null;

    [Header("InputController")]
    [SerializeField] private MenuInputController inputsUI;

    private bool isMenuOpen = false;

    private void Awake()
    {
        inputsUI.onPauseInput += TogglePauseMenuState;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.anyKeyDown)
    //    {
    //        if (Input.GetKeyDown(openMenu))
    //        {
    //            TogglePauseMenuState();
    //        }
    //    }
    //}

    public void TogglePauseMenuState()
    {
        if (isMenuOpen)
        {
            canvas.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        else
        {
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

    private void OnDestroy()
    {
        inputsUI.onPauseInput -= TogglePauseMenuState;
    }
}
