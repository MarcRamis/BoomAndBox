using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLogic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneNameToChangeOnPlay = "Level1";

    public void Play()
    {
        SceneManager.LoadScene(sceneNameToChangeOnPlay);
    }

}
