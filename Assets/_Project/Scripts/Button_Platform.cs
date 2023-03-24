using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DG.Tweening;
using UnityEngine.Events;
using System;

//---Events
[Serializable]
public class PuzzleEvent : UnityEvent<int, int> { }

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

    [Header("Analytics")]
    [SerializeField] private int puzzleID = 0;
    [SerializeField] private int elementPuzzleOrder = 0;

    [Header("Unity Events")]
    [SerializeField] UnityEvent End_Event;
    //[SerializeField] PuzzleEvent Puzzle_Event;

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
        //if(Puzzle_Event == null)
        //    Puzzle_Event = new PuzzleEvent();
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
                    Transform tempTrans = this.transform;
                    Debug.Log(tempTrans.localRotation);
                    tempTrans.localRotation = Quaternion.Euler(new Vector3(tempTrans.rotation.eulerAngles.x, tempTrans.rotation.eulerAngles.y + 180, tempTrans.rotation.eulerAngles.z));
                    Debug.Log(tempTrans.localRotation);
                    transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, tempTrans.rotation.eulerAngles, Time.deltaTime);
                    //Debug.Log(tempTrans.rotation);
                    //Vector3 to = new Vector3(transform.rotation.eulerAngles.x, tempTrans.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    //transform.localEulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
                    break;
            }
            JSON_Creator.Instance.Puzzle(puzzleID, elementPuzzleOrder);
            //Puzzle_Event?.Invoke(puzzleID, elementPuzzleOrder);
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
