using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class ShooterFeedbackController : FeedbackController
{
    [Header("Visual Effects")]
    [SerializeField] private GameObject prefabDestroyedShooter;

    /////////// BEING DESTROYED
    public void PlayBeingDestroyed()
    {
        Instantiate(prefabDestroyedShooter, transform.position, prefabDestroyedShooter.transform.rotation);
    }
}