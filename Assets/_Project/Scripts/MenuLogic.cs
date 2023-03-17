using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneNameToChangeOnPlay = "Level1";

    public void Play()
    {
        SceneManager.LoadScene(sceneNameToChangeOnPlay);
    }

    public void Options()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

}
