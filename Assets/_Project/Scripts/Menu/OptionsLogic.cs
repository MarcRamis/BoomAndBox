using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsLogic : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;

    [SerializeField] private EventsSystem eventsSystem;

    [Header("Button to return")]
    [SerializeField] private GameObject playButton;

    [Header("Variables")]
    [SerializeField] private float initMasterVolume = 0.4f;

    [SerializeField] private float timeToWait = 0.5f;

    private float timer = 0.0f;
    private bool wait = true;

    private void Awake()
    {
        wait = true;
        timer = 0.0f;
    }

    void Start()
    {
        audioMixer.SetFloat("MasterVolume", initMasterVolume);
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
    public void MasterMusicControl(float _sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(_sliderValue) * 20.0f);
    }

    public void CloseOptions()
    {
        if(!wait)
        {
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(playButton, new BaseEventData(eventSystem));
            this.gameObject.SetActive(false);
        }
    }

}
