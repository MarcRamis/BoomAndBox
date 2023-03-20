using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DG.Tweening;
using UnityEngine.Events;

public class Button_Platform : MonoBehaviour, IColorizer
{
    [Header("Platform list")]
    [SerializeField] private GameObject[] platformsToChange;

    [Header("Settings")]
    [SerializeField] private PlatformAction actionToDo;
    [SerializeField] private bool isColorCorrect = true;
    [SerializeField] private float rotationSpeed = 25.0f;
    [SerializeField] private float angleToHave = 0.0f;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks hitFeedback;

    [Header("Materials")]
    [SerializeField] private Material matBaseColor1;
    [SerializeField] private Material matBaseColor2;

    [Header("Unity Events")]
    [SerializeField] UnityEvent End_Event;

    //Private variables
    private MeshRenderer matBaseColor;
    private enum PlatformAction
    {
        Move,
        End
    };
    private bool timer = false;

    private void Awake()
    {
        matBaseColor = transform.Find("boton").GetComponent<MeshRenderer>();

        if (!isColorCorrect)
        {
            matBaseColor.material = matBaseColor2;
        }

        if (End_Event == null)
            End_Event = new UnityEvent();

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
                case PlatformAction.End:
                    isColorCorrect = false;
                    End_Event?.Invoke();
                    Vector3 to = new Vector3(transform.rotation.eulerAngles.x, angleToHave, transform.rotation.eulerAngles.z);

                    transform.localEulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
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
