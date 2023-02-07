using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using System;

public class ThrowingSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform standPosition;
    [SerializeField] private Transform toAttach;
    [SerializeField] public GameObject objectToThrow;
    [HideInInspector] public Companion companion;
    private PlayerMovementSystem pm;
    
    [Header("Throw")]
    [SerializeField] private float throwForce;
    [SerializeField] private float throwLargeForce;
    [SerializeField] private float throwCooldown = 0.25f;
    private bool readyToThrow = true;
    
    [Header("Return")]
    [SerializeField] private float returnTime;
    [SerializeField] private AnimationCurve returnCurveSmooth;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks comebackFeedback;
    [SerializeField] private MMFeedbacks throwingFeedback;
    [SerializeField] private MMFeedbacks exclamationFeedback;

    // Constant variables
    private const float targetNearDistance = 0.2f;

    // Internal variables
    private float elapsedTime;
    private Vector3 startPosition;

    // Start
    private void Start()
    {
        // Get components
        pm = GetComponent<PlayerMovementSystem>();
        companion = objectToThrow.GetComponent<Companion>();
        
        // Init Inputs
        pm.myInputs.OnThrowPerformed += DoThrow;
        pm.myInputs.OnReturnPerformed += DoReturn;

        standPosition.position = companion.transform.position;
    }

    private void DoThrow()
    {
        // Throw BOX CHARACTER 
        if (readyToThrow)
        {
            // Throw large
            if (pm.isAiming)
            {
                // change state
                companion.SetNewState(ECompanionState.THROW_LARGE);
                // Do Throw
                Throw(cam.transform.forward, throwLargeForce);
            }
            else
            {
                // Throw short -- dash
                if (companion.state == ECompanionState.ATTACHED)
                {
                    // change state
                    companion.SetNewState(ECompanionState.THROW);
                    // Do Throw
                    Throw(cam.transform.forward, throwForce);
                }
                else if (companion.state == ECompanionState.THROW)
                {
                    companion.SetNewState(ECompanionState.RETAINED);
                }

                else if (companion.state == ECompanionState.RETAINED)
                {
                    companion.SetNewState(ECompanionState.COMEBACK);
                }
            }
        }
    }

    private void DoReturn()
    {
        // Comeback
        if (companion.state != ECompanionState.ATTACHED)
        {
            companion.SetNewState(ECompanionState.COMEBACK);
        }
    }

    // Fixed Update
    private void FixedUpdate()
    {
        if (companion.state != ECompanionState.COMEBACK) return;

        ComeBackInterp();
    }
    
    // Functions
    private void Throw(Vector3 forceDirection, float force)
    {
        // Preferences
        readyToThrow = false;
        objectToThrow.transform.SetParent(null);
        throwingFeedback.PlayFeedbacks();

        // Get rigidbody component
        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();

        // Change preferences
        companion.HandleState();
        
        // Add force
        Vector3 forceToAdd = forceDirection * force;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
    private void ComeBackInterp()
    {
        startPosition = objectToThrow.transform.position;
        elapsedTime += Time.fixedDeltaTime;
        float percentageComplete = elapsedTime / returnTime;
        
        objectToThrow.transform.position = Vector3.Lerp(startPosition, standPosition.position, returnCurveSmooth.Evaluate(percentageComplete));

        if (Vector3.Distance(standPosition.position, objectToThrow.transform.position) < targetNearDistance)
        {
            ResetThrow();
        }
    }
    private void ResetThrow()
    {
        // This is the reset of the BOX CHARACTER
        companion.SetNewState(ECompanionState.ATTACHED);
        objectToThrow.transform.SetParent(toAttach);
        elapsedTime = 0;
        comebackFeedback.PlayFeedbacks();

        // timer to throw again
        Invoke(nameof(ResetThrowCooldown), throwCooldown);
    }
    private void ResetThrowCooldown()
    {
        readyToThrow = true;
        exclamationFeedback.PlayFeedbacks();
    }
}