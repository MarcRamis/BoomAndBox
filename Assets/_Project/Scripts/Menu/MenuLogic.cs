using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneNameToChangeOnPlay = "Level1";
    [SerializeField] private float timeToWait = 0.5f;

    [Header("Play Menu")]
    [SerializeField] private GameObject playMenu = null;
    [SerializeField] private GameObject playButton;

    [Header("Options")]
    [SerializeField] private GameObject options = null;
    [SerializeField] private GameObject optionSlider;
    [SerializeField] private AudioMixer audioMixer;
    [Header("Credits")]
    [SerializeField] private GameObject credits = null;
    [SerializeField] private GameObject returnButton = null;

    private float timer = 0.0f;
    private bool wait = true;

    private void Awake()
    {
        wait = true;
        timer = 0.0f;
    }

    private void Update()
    {
        if(wait)
        {
            timer += Time.deltaTime;

            if(timer > timeToWait ) 
            {
                wait = false;
                timer = 0.0f;
            }

        }
    }

    public void StartWait()
    {
        wait = true;
    }

    //-------MENU-------//
    public void Play()
    {
        if(!wait)
        {
            SceneManager.LoadScene(sceneNameToChangeOnPlay);
            StartWait();
        }
            
    }

    public void Options()
    {
        if(!wait)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(optionSlider, new BaseEventData(eventSystem));

            options.SetActive(true);
            StartWait();
        }
        
    }

    public void Exit()
    {
        if(!wait)
        {
            Application.Quit();
        }
            
    }

    public void Credits()
    {
        if (!wait)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(returnButton, new BaseEventData(eventSystem));
            credits.SetActive(true);
            StartWait();
        }
            
    }

    //-------OPTIONS-------//
    public void MasterMusicControl(float _sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(_sliderValue) * 20.0f);
    }

    public void CloseOptions()
    {
        if (!wait)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(playButton, new BaseEventData(eventSystem));
            options.SetActive(false);
            StartWait();
        }
    }

    //-------CREDITS-------//
    public void CloseCredits()
    {
        if(!wait)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(playButton, new BaseEventData(eventSystem));
            credits.SetActive(false);
            StartWait();
        }
    }
}
