using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneNameToChangeOnPlay = "Level1";

    [Header("Objects")]
    [SerializeField] private GameObject options = null;

    public void Play()
    {
        SceneManager.LoadScene(sceneNameToChangeOnPlay);
    }

    public void Options()
    {
        options.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
