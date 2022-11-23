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
    [SerializeField] private KeyCode returnKey = KeyCode.Mouse1;
    
    [Header("Throw")]
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;
    [SerializeField] private float maxCounterToBeThrowed;
    [SerializeField] private bool canBeRedirected;

    [Header("Return")]
    [SerializeField] public EReturnType returnType;
    [SerializeField] private float comebackForce;
    [SerializeField] private float distanceToTargetForSlowReturn;
    [SerializeField] private float multiplierSlowSpeed;
    [SerializeField] private float returnTime;
    [SerializeField] private AnimationCurve returnCurveSmooth;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks comebackFeedback;
    [SerializeField] private MMFeedbacks throwingFeedback;

    // Constant variables
    private const float targetNearDistance = 0.2f;

    // Internal variables
    private Vector3 saveFirstThrowDir;
    private int throwsCounter = 0;
    private float elapsedTime;
    private Vector3 startPosition;

    public enum EReturnType { FORCE, INTERPOLATION };

    // Start
    private void Start()
    {
        toL = objectToThrow.GetComponent<ThrowingObj>();
    }

    // Update
    private void Update()
    {
        // Throw BOX CHARACTER 
        // This could be a switch but i'm prototyping
        if(Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.ATTACHED)
        {
            objectToThrow.transform.SetParent(null);
            Throw(cam.transform.forward);
            saveFirstThrowDir = cam.transform.forward;
            throwingFeedback.PlayFeedbacks();
        }
        else if(Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.THROW)
        {
            toL.SetNewState(ThrowingObj.EThrowingState.RETAINED);
        }

        else if (Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.RETAINED)
        {
            if(!canBeRedirected) Throw(saveFirstThrowDir);
            else Throw(cam.transform.forward);
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

        switch (returnType)
        {
            case EReturnType.FORCE:
                ComeBackForce();
                break;
            case EReturnType.INTERPOLATION:
                ComeBackInterp();
                break;
        }
    }

    // Functions
    private void Throw(Vector3 forceDirection)
    {
        // Preferences
            // change state
        if (throwsCounter > maxCounterToBeThrowed)
        {
            toL.SetNewState(ThrowingObj.EThrowingState.COMEBACK);
        }
        else
        {
            throwsCounter++;
            toL.SetNewState(ThrowingObj.EThrowingState.THROW);
        }

            // get rigidbody component
        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();
            // change preferences
        projectileRb.useGravity = false;
        projectileRb.isKinematic = false;
        projectileRb.interpolation = RigidbodyInterpolation.Interpolate;
        projectileRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
    private void ComeBackForce()
    {
        Vector3 direction = standPosition.position - objectToThrow.transform.position;
        direction = direction.normalized * comebackForce;

        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();
        projectileRb.velocity = direction;
        
        if (Vector3.Distance(standPosition.position, objectToThrow.transform.position) < distanceToTargetForSlowReturn)
        {
            projectileRb.velocity = direction * multiplierSlowSpeed;
        }

        TargetIsNear();
    }

    private void ComeBackInterp()
    {
        startPosition = objectToThrow.transform.position;
        elapsedTime += Time.fixedDeltaTime;
        float percentageComplete = elapsedTime / returnTime;
        
        objectToThrow.transform.position = Vector3.Lerp(startPosition, standPosition.position, returnCurveSmooth.Evaluate(percentageComplete));
        
        TargetIsNear();
    }

    private void TargetIsNear()
    {
        if (Vector3.Distance(standPosition.position, objectToThrow.transform.position) < targetNearDistance)
        {
            // This is the reset of the BOX CHARACTER
            toL.SetNewState(ThrowingObj.EThrowingState.ATTACHED);
            objectToThrow.transform.SetParent(toAttach);
            throwsCounter = 0;
            elapsedTime = 0;

            comebackFeedback.PlayFeedbacks();
        }
    }
}