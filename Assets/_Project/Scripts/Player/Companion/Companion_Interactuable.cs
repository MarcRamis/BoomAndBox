using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Companion_Interactuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private GameObject bubbleControl;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private BoxCollider triggerCollider2;
    [HideInInspector] private GameObject companion;
    private Companion companionScript;
    private Player playerScript;
    [SerializeField] private GameObject objectToPositionate;
    //[SerializeField] private GameObject masterLevel;

    [Header("Unity Event")]
    [SerializeField] private UnityEvent End_Event;

    float elapsedTime;

    private void Awake()
    {
        companion = ReferenceSingleton.Instance.companion;
        companionScript = ReferenceSingleton.Instance.companionScript;
        playerScript = ReferenceSingleton.Instance.playerScript;

        if (End_Event == null)
            End_Event = new UnityEvent();

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
        triggerCollider2.enabled = false;
        bubbleControl.SetActive(false);
        playerScript.currentInteraction = null;
        //masterLevel.GetComponent<IEvent>().EventAction(this.gameObject);
        Invoke(nameof(ShowCompanion), 0.05f);
        StartCoroutine(InterpolationUtils.Interpolate(gameObject.transform, gameObject.transform.position, objectToPositionate.transform.position, 0.1f, OnInterpolationFinished));
        
    }

    private void OnInterpolationFinished()
    {
        companionScript.SetNewState(ECompanionState.ATTACHED);
        End_Event?.Invoke();
        Destroy(gameObject);
    }

    private void ShowCompanion()
    {
        companion.SetActive(true);
        Debug.Log("ShowCompanion");
        Debug.Log(companion.activeSelf);
        Debug.Log(companion.transform.position);
        Debug.Log(playerScript.transform.position);
    }

}
