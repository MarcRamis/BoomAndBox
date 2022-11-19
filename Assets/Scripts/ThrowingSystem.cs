using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThrowingSystem : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public Transform standPosition;
    public Transform toAttach;
    public GameObject objectToThrow;
    [HideInInspector] public ThrowingObj toL;
    
    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;
    
    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public KeyCode returnKey = KeyCode.Mouse1;
    public float throwForce;
    public float comebackForce;
    public float throwUpwardForce;
    
    bool readyToThrow;
    public Transform lastCoinPos;
    private Vector3 saveFirstThrowDir;

    private int throwsCounter = 0; 

    private void Start()
    {
        toL = objectToThrow.GetComponent<ThrowingObj>();
    }

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

    private void FixedUpdate()
    {
        if (toL.m_State != ThrowingObj.EThrowingState.COMEBACK) return;
            
        ComeBack();
    }
    
    private void Throw(Vector3 forceDirection)
    {
        // Preferences
            // change state
        if (throwsCounter > toL.maxCounterToBeThrowed)
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
        projectileRb.useGravity = true;
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
        
        if (Vector3.Distance(standPosition.position, objectToThrow.transform.position) < 4)
        {
            projectileRb.velocity = direction * 0.2f;
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