using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Return")]
    [SerializeField] private float comebackForce;
    [SerializeField] private float distanceToTargetForSlowReturn;
    [SerializeField] private float multiplierSlowSpeed;

    // Internal variables
    private Vector3 saveFirstThrowDir;
    private int throwsCounter = 0; 

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
        }
        else if(Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.THROW)
        {
            toL.SetNewState(ThrowingObj.EThrowingState.RETAINED);
        }

        else if (Input.GetKeyDown(throwKey) && toL.m_State == ThrowingObj.EThrowingState.RETAINED)
        {
            Throw(saveFirstThrowDir);
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
            
        ComeBack();
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
    private void ComeBack()
    {
        Vector3 direction = standPosition.position - objectToThrow.transform.position;
        direction = direction.normalized * comebackForce;

        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();
        projectileRb.velocity = direction;
        
        if (Vector3.Distance(standPosition.position, objectToThrow.transform.position) < distanceToTargetForSlowReturn)
        {
            projectileRb.velocity = direction * multiplierSlowSpeed;
        }
        
        if (Vector3.Distance(standPosition.position, objectToThrow.transform.position) < 0.2)
        {
            // This is the reset of the BOX CHARACTER
            toL.SetNewState(ThrowingObj.EThrowingState.ATTACHED);
            objectToThrow.transform.SetParent(toAttach);
            throwsCounter = 0;
        }
    }
}