using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Rendering.PostProcessing;

public class Button_Platform2 : MonoBehaviour, IDamageable
{
    [Header("Platform list")]
    [SerializeField] private GameObject[] platformsToChange;

    [Header("Settings")]
    [SerializeField] private PlatformAction actionToDo;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks hitFeedback;

    public PostProcessVolume mainPostProcess;

    private void Awake()
    {
        mainPostProcess = GameObject.FindGameObjectWithTag("Postprocess").GetComponent<PostProcessVolume>();
    }

    //Private variables
    private enum PlatformAction
    {
        Move
    };
    private bool timer = false;

    public int Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Companion" && !timer)
        {
            //hitFeedback.PlayFeedbacks();
            switch (actionToDo)
            {
                case PlatformAction.Move:
                    foreach (var platform in platformsToChange)
                    {
                        platform.GetComponentInChildren<MoveablePlatform>().ChangeMoveableState();
                    }
                    break;
            }
        }
    }

    public void Damage(int damageAmount)
    {
        hitFeedback.PlayFeedbacks();
    }
}
