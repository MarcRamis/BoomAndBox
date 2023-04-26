using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Rendering.PostProcessing;
using System;
using UnityEditor;

public class Button_Platform2 : MonoBehaviour, IDamageable, IEvent
{
    public delegate void OnActivationAction();
    public static event OnActivationAction OnActivation;

    [Header("Events")]
    [SerializeField] private GameObject[] eventStart;
    [SerializeField] private GameObject[] eventEnds;
    [SerializeField] private GameObject[] eventAction;

    [Header("Platform list to Appear")]
    [SerializeField] private GameObject[] platformsToAppear;

    [Header("Platform list to Move")]
    [SerializeField] private GameObject[] platformsToMove;

    [Header("Platform list to ChangeColor")]
    [SerializeField] private GameObject[] platformsToChangeColor;

    [Header("Settings")]
    [SerializeField] private PlatformAction actionToDo;
    [SerializeField] private Color color1 = Color.white;
    [SerializeField] private Color color2 = Color.white;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks hitFeedback;    

    public PostProcessVolume mainPostProcess;

    private void Awake()
    {
        mainPostProcess = GameObject.FindGameObjectWithTag("Postprocess").GetComponent<PostProcessVolume>();
    }

    //Private variables
    [Flags]
    private enum PlatformAction
    {
        NONE = 0,
        Move = 1 << 1,
        Activate = 1 << 2,
        ChangeColor = 1 << 3,
    };
    private bool timer = false;

    public int Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Companion" && !timer)
        {
            //hitFeedback.PlayFeedbacks();
            foreach(PlatformAction option in Enum.GetValues(typeof(PlatformAction)))
            {
                switch (option)
                {
                    case PlatformAction.Move:
                        foreach (var platform in platformsToMove)
                        {
                            platform.GetComponentInChildren<MoveablePlatform>().ChangeMoveableState();
                        }
                        break;

                    case PlatformAction.Activate:
                        if (platformsToAppear[0] != null)
                        {
                            foreach (var platform in platformsToAppear)
                            {
                                platform.SetActive(!platform.activeSelf);
                            }
                        }
                        break;
                    case PlatformAction.ChangeColor:
                        foreach (var platform in platformsToChangeColor)
                        {
                            IColorizer tempScript = platform.GetComponent<IColorizer>();

                            if(tempScript != null)
                                tempScript.ChangeMaterial();



                        }
                        break;
                }
            }
            timer = true;
        }
    }

    public void ResetTimer()
    {
        timer = false;
    }

    public void Damage(int damageAmount)
    {
        if(!timer)
        {
            hitFeedback.PlayFeedbacks();
            if (eventAction.Length > 0)
                foreach (GameObject objectActivate in eventAction)
                {
                    IEvent eventScript = objectActivate.gameObject.GetComponent<IEvent>();
                    if (eventScript != null)
                    {
                        eventScript.EventAction(this.gameObject);
                    }
                }
        }
        
    }

    //Events
    public void EventStarts()
    {           
                
    }
    public void EventEnds()
    {

    }
    public void EventAction()
    {

    }
    public void EventAction(GameObject _gameobject)
    {

    }

    public void Knockback(float force)
    {
        throw new NotImplementedException();
    }
}
