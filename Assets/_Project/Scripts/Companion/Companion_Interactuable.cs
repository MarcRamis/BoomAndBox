using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion_Interactuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private GameObject bubbleControl;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private GameObject companion;
    [SerializeField] private Companion companionScript;
    [SerializeField] private GameObject objectToPositionate;
    
    float elapsedTime;

    private void Awake()
    {
        companion = GameObject.FindGameObjectWithTag("Companion");
        companionScript = companion.GetComponent<Companion>();
    }

    public void InteractStarts()
    {
        if (bubbleControl != null)
        { 
            bubbleControl.SetActive(true);
        }
    }
    
    public void InteractEnds()
    {
        if (bubbleControl != null)
        {
            bubbleControl.SetActive(false);
        }
    }
    
    public void MakeInteraction()
    {
        triggerCollider.enabled = false;
        bubbleControl.SetActive(false);
        StartCoroutine(InterpolationUtils.Interpolate(gameObject.transform, gameObject.transform.position, objectToPositionate.transform.position, 0.1f, OnInterpolationFinished));
    }
    private void OnInterpolationFinished()
    {
        companion.SetActive(true);
        companionScript.SetNewState(ECompanionState.ATTACHED);
        Destroy(gameObject);
    }
}
