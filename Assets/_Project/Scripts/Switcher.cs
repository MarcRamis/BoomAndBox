using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    /// <summary>
    /// Put the group of platforms that you want in every container on the References part.
    /// In the settings part you can see the state the group of platforms swap to an other specified state.
    /// </summary>
    
    [Header("References")]
    [SerializeField] private Platform[] firstGroupPlatforms;
    [SerializeField] private Platform[] secoundGroupPlatforms;

    [Header("Settings")]
    [SerializeField] private Platform.EPlatformState firstGroupPlaftormsSwapState;
    [SerializeField] private Platform.EPlatformState secondGroupPlatformsSwapState;

    private bool isActivated = false;

    // Functions
    private void Switch()
    {
        if (isActivated) isActivated = false;
        else isActivated = true;
        
        foreach (Platform p in firstGroupPlatforms)
        {
            ChangePlatformState(p, firstGroupPlaftormsSwapState, secondGroupPlatformsSwapState);
        }

        foreach (Platform p in secoundGroupPlatforms)
        {
            ChangePlatformState(p, secondGroupPlatformsSwapState, firstGroupPlaftormsSwapState);
        }
    }

    private void ChangePlatformState(Platform p, Platform.EPlatformState st1, Platform.EPlatformState st2)
    {
        if (!isActivated)
        {
            p.ChangeState(st1);
            p.HandleState();
        }
        else
        {
            p.ChangeState(st2);
            p.HandleState();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Companion"))
        {
            Switch();
            
            GameObject companion = GameObject.FindGameObjectWithTag("Companion");
            companion.GetComponent<ThrowingObj>().SetNewState(ThrowingObj.EThrowingState.COMEBACK);
        }
    }
}
