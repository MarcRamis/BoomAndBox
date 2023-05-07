using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion_Interactuable : MonoBehaviour, IInteractuable
{
    [SerializeField] private GameObject bubbleControl;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private BoxCollider triggerCollider2;
    [SerializeField] private GameObject companion;
    private Companion companionScript;
    [SerializeField] private GameObject player;
    private Player playerScript;
    [SerializeField] private GameObject objectToPositionate;
    [SerializeField] private GameObject masterLevel;

    float elapsedTime;

    private void Awake()
    {
        companionScript = ReferenceSingleton.Instance.companionScript;
        playerScript = ReferenceSingleton.Instance.playerScript;
        //companion = GameObject.FindGameObjectWithTag("Companion");
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        companionScript = companion.GetComponent<Companion>();
        playerScript = player.GetComponent<Player>();
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
        masterLevel.GetComponent<IEvent>().EventAction(this.gameObject);
        StartCoroutine(InterpolationUtils.Interpolate(gameObject.transform, gameObject.transform.position, objectToPositionate.transform.position, 0.1f, OnInterpolationFinished));
    }

    private void OnInterpolationFinished()
    {
        companion.SetActive(true);
        companionScript.SetNewState(ECompanionState.ATTACHED);
        Destroy(gameObject);
    }
}
