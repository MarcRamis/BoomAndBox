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


    private bool isMenuOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(openMenu))
            {
                TogglePauseMenuState();
            }
        }
    }

    public void TogglePauseMenuState()
    {
        if (isMenuOpen)
        {
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            canvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        isMenuOpen = !isMenuOpen;
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
