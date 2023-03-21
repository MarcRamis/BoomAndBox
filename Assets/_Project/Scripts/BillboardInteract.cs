using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardInteract : MonoBehaviour, IInteractuable
{
    [SerializeField] private GameObject bubbleControl;
    //[SerializeField] private BoxCollider[] triggerEnter;
    //[SerializeField] private BoxCollider[] triggerExit;

    [SerializeField] private GameObject player;
    private Player playerScript;

    private void Awake()
    {
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
            playerScript.currentInteraction = null;
            bubbleControl.SetActive(false);
        }
    }

    public void MakeInteraction()
    {
        return;
    }

    private void OnDestroy()
    {
        playerScript.currentInteraction = null;
    }

}
