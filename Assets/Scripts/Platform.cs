using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    /// <summary> 
    /// What does every platform type
    /// Default --> Stands
    /// Tangible Non Tangible --> Swap between being tangible or non tangible. They need to be added on a switcher
    /// </summary>

    [Header("References")]
    private Collider m_Collider;
    [SerializeField] private Material solidMaterial;
    [SerializeField] private Material tangibleMaterial;

    [SerializeField] public enum EPlatformType { DEFAULT, TANGNONTANG}
    [Header("Settings")]
    [SerializeField] private EPlatformType platformType;
    
    [SerializeField] public enum EPlatformState { SOLID, NONTANGIBLE,}
    [SerializeField] private EPlatformState platformState;
    
    // Awake
    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        HandleState();
    }

    // Functions
    public void HandleState()
    {
        switch (platformState)
        {
            case EPlatformState.SOLID:

                m_Collider.enabled = true;
                GetComponent<Renderer>().material= solidMaterial; 
                break;
            
            case EPlatformState.NONTANGIBLE:

                m_Collider.enabled = false;
                GetComponent<Renderer>().material = tangibleMaterial;
                break;
        }
    }

    public void ChangeState(EPlatformState _state)
    {
        platformState = _state;
    }
}