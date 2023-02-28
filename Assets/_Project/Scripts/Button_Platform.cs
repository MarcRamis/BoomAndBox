using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Button_Platform : MonoBehaviour
{
    [Header("Platform list")]
    [SerializeField] private GameObject[] platformsToChange;

    [Header("Settings")]
    [SerializeField] private PlatformAction actionToDo;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks hitFeedback;

    //Private variables
    private enum PlatformAction
    {
        Move
    };
    private bool timer = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Companion" && !timer)
        {
            hitFeedback.PlayFeedbacks();
            switch (actionToDo)
            {
                case PlatformAction.Move:
                    foreach (var platform in platformsToChange)
                    {
                        if(!platform.GetComponentInChildren<MoveablePlatform>().GetIsOtherColor())
                        {
                            platform.GetComponentInChildren<MoveablePlatform>().ChangeMoveableState();
                        }    
                    }
                    break;
            }
        }
    }
}
