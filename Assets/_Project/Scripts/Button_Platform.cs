using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Button_Platform : MonoBehaviour, IColorizer
{
    [Header("Platform list")]
    [SerializeField] private GameObject[] platformsToChange;

    [Header("Settings")]
    [SerializeField] private PlatformAction actionToDo;
    [SerializeField] private bool isColorCorrect = true; 

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks hitFeedback;

    [Header("Materials")]
    [SerializeField] private Material matBaseColor1;
    [SerializeField] private Material matBaseColor2;

    //Private variables
    private MeshRenderer matBaseColor;
    private enum PlatformAction
    {
        Move
    };
    private bool timer = false;

    private void Awake()
    {
        matBaseColor = transform.Find("boton").GetComponent<MeshRenderer>();

        if (!isColorCorrect)
        {
            matBaseColor.material = matBaseColor2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Companion" && !timer && isColorCorrect)
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

    public void ChangeMaterial()
    {
        isColorCorrect = !isColorCorrect;

        if (isColorCorrect)
        {
            matBaseColor.material = matBaseColor1;
        }
        else
        {
            matBaseColor.material = matBaseColor2;
        }
    }
}
