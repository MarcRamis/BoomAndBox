using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneNameToChangeOnPlay = "Level1";

    [Header("Objects")]
    [SerializeField] private GameObject options = null;

    [Header("Button to options")]
    [SerializeField] private GameObject optionSlider;

    public void Play()
    {
        SceneManager.LoadScene(sceneNameToChangeOnPlay);
    }

    public void Options()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(optionSlider, new BaseEventData(eventSystem));

        options.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
