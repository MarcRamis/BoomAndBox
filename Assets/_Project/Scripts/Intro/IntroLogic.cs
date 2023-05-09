using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLogic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneNameToChangeOnPlay = "Level1";
    [SerializeField] private float timeToWait = 0.1f;

    private float timer = 0.0f;
    private bool wait = true;

    private void Awake()
    {
        wait = true;
        timer = 0.0f;
    }

    private void Update()
    {
        if (wait)
        {
            timer += Time.deltaTime;

            if (timer > timeToWait)
            {
                wait = false;
                timer = 0.0f;
            }

        }
    }
    public void Play()
    {
        if(!wait)
            SceneManager.LoadScene(sceneNameToChangeOnPlay);
    }

}
