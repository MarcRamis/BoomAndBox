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
    [SerializeField] private Transform standPositionThrow;
    [SerializeField] private Transform toAttach;
    [SerializeField] public GameObject objectToThrow;
    [HideInInspector] public Companion companion;
    [HideInInspector] private PlayerMovementSystem pm;
    
    [Header("Throw")]
    [SerializeField] private float throwForce;
    [SerializeField] private float throwLargeForce;
    [SerializeField] private float throwCooldown = 0.25f;
    [SerializeField] private float justThrowCooldown = 0.05f;
    [HideInInspector] public bool readyToThrow = true;
    [HideInInspector] public bool justThrow = false;
    [HideInInspector] public bool isAiming;
    
    [Header("Return")]
    [SerializeField] private float returnTime;
    [SerializeField] private AnimationCurve returnCurveSmooth;

    //Inputs
    [HideInInspector] private PlayerInputController myInputs;
    //Feedback
    [HideInInspector] private PlayerFeedbackController playerFeedbackController;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks comebackFeedback;
    [SerializeField] private MMFeedbacks exclamationFeedback;

    // Constant variables
    [HideInInspector] private const float targetNearDistance = 0.2f;

    // Internal variables
    [HideInInspector] private float elapsedTime;
    [HideInInspector] private Vector3 startPosition;

    private void Awake()
    {
        pm = GetComponent<PlayerMovementSystem>();
        companion = objectToThrow.GetComponent<Companion>();
        myInputs = GetComponent<PlayerInputController>();
        playerFeedbackController = GetComponent<PlayerFeedbackController>();

        myInputs.OnZoomPerformed += DoAim;
        myInputs.OnThrowPerformed += DoThrow;
        myInputs.OnReturnPerformed += DoReturn;

        standPosition.position = companion.transform.position;
    }

    private void FixedUpdate()
    {
        companion.RotateModel(cam.forward);

        if (companion.state != ECompanionState.COMEBACK) return;

        InterpolateComeback();
    }

    private void DoAim()
    {
        if (companion.state == ECompanionState.ATTACHED)
        {
            isAiming = !isAiming;
            pm.isAiming = !pm.isAiming;

            // change companion properties to stay as aiming
            companion.playerAiming = !companion.playerAiming;

            if (isAiming)
            {
                StartCoroutine(InterpolationUtils.Interpolate(objectToThrow.transform, objectToThrow.transform.position, standPositionThrow.position, 0.01f, null));
            }
            else
            {
                StartCoroutine(InterpolationUtils.Interpolate(objectToThrow.transform, objectToThrow.transform.position, standPosition.position, 0.01f, null));
            }
        }
    }

    private void DoThrow()
    {
        // Throw BOX CHARACTER 
        if (readyToThrow && companion.state != ECompanionState.NONE)
        {
            // Throw large
            if (pm.isAiming)
            {
                // change state
                companion.SetNewState(ECompanionState.THROW_LARGE);

                // Do Throw
                //Vector3 dir = pm.lookAt.position - transform.position;
                //dir.Normalize();
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
    // Functions
    private void Throw(Vector3 forceDirection, float force)
    {
        // effect
        playerFeedbackController.PlayThrowFeedback();

        // Preferences
        readyToThrow = false;
        objectToThrow.transform.SetParent(null);

        justThrow = true;
        Invoke(nameof(ResetJustThrow), justThrowCooldown);

        // Get rigidbody component
        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();

        // Change preferences
        companion.HandleState();
        companion.ApplyThrow(forceDirection, force);
    }
    private void InterpolateComeback()
    {
        startPosition = objectToThrow.transform.position;
        elapsedTime += Time.fixedDeltaTime;
        float percentageComplete = elapsedTime / returnTime;
        

        if (isAiming)
        {
            objectToThrow.transform.position = Vector3.Lerp(startPosition, standPositionThrow.position, returnCurveSmooth.Evaluate(percentageComplete));

            if (Vector3.Distance(standPositionThrow.position, objectToThrow.transform.position) < targetNearDistance)
            {
                ResetThrow();
            }

        }
        else
        {
            objectToThrow.transform.position = Vector3.Lerp(startPosition, standPosition.position, returnCurveSmooth.Evaluate(percentageComplete));

            if (Vector3.Distance(standPosition.position, objectToThrow.transform.position) < targetNearDistance)
            {
                ResetThrow();
            }
        }
    }

    private void ResetJustThrow()
    {
        justThrow = false;
    }
    private void ResetThrow()
    {
        // This is the reset of the BOX CHARACTER
        companion.SetNewState(ECompanionState.ATTACHED);
        objectToThrow.transform.SetParent(toAttach);
        companion.ResetInitialProperties(true);
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