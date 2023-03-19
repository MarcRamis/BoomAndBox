using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsLogic : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;

    [Header("Variables")]
    [SerializeField] private float initMasterVolume = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("MasterVolume", initMasterVolume);
    }

    public void MasterMusicControl(float _sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(_sliderValue) * 20.0f);
    }

    public void CloseOptions()
    {
        this.gameObject.SetActive(false);
    }

}
