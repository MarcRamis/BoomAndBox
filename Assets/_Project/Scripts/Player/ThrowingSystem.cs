using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class ThrowingSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform standPosition;
    [SerializeField] private Transform toAttach;
    [SerializeField] public GameObject objectToThrow;
    [HideInInspector] public ThrowingObj toL;
    
    [Header("Inputs")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode throwLargeKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode returnKey = KeyCode.LeftControl;
    
    [Header("Throw")]
    [SerializeField] private float throwForce;
    [SerializeField] private float throwLargeForce;
    [SerializeField] private float throwCooldown = 0.25f;
    private bool readyToThrow = true;

    [Header("Return")]
    [SerializeField] private float returnTime;
    [SerializeField] private AnimationCurve returnCurveSmooth;

    [Header("Cooldown")]

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks comebackFeedback;
    [SerializeField] private MMFeedbacks throwingFeedback;

    // Constant variables
    private const float targetNearDistance = 0.2f;

    // Internal variables
    private float elapsedTime;
    private Vector3 startPosition;

    // Start
    private void Start()
    {
        toL = objectToThrow.GetComponent<ThrowingObj>();
    }

    // Update
    private void Update()
    {
        // Throw BOX CHARACTER 
        if (readyToThrow)
        {
            if (Input.GetKey(throwLargeKey))
            {
                // Falta feedback para cuando estás apuntando

                if (Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.ATTACHED)
                {
                    // change state
                    toL.SetNewState(ThrowingObj.EThrowingState.THROW_LARGE);
                    // Do Throw
                    Throw(cam.transform.forward, throwLargeForce);
                }
            }
            else
            {
                if (Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.ATTACHED)
                {
                    // change state
                    toL.SetNewState(ThrowingObj.EThrowingState.THROW);
                    // Do Throw
                    Throw(cam.transform.forward, throwForce);
                }
                else if (Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.THROW)
                {
                    toL.SetNewState(ThrowingObj.EThrowingState.RETAINED);
                }

                else if (Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.RETAINED)
                {
                    toL.SetNewState(ThrowingObj.EThrowingState.COMEBACK);
                }
            }
        }

        // Comeback to BOOM CHARACTER
        if (Input.GetKeyDown(returnKey) && toL.m_State != ThrowingObj.EThrowingState.ATTACHED)
        {
            toL.SetNewState(ThrowingObj.EThrowingState.COMEBACK);
        }
    }

    // Fixed Update
    private void FixedUpdate()
    {
        if (toL.m_State != ThrowingObj.EThrowingState.COMEBACK) return;

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
        toL.StateHandler();
        
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
        toL.SetNewState(ThrowingObj.EThrowingState.ATTACHED);
        objectToThrow.transform.SetParent(toAttach);
        elapsedTime = 0;
        comebackFeedback.PlayFeedbacks();

        // timer to throw again
        Invoke(nameof(ResetThrowCooldown), throwCooldown);
    }
    private void ResetThrowCooldown()
    {
        readyToThrow = true;
    }
}