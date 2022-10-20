using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    private Throwing_Obj_Logic toL;
    
    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    private int clickCounter = 0;
    public float throwForce;
    public float throwUpwardForce;
    
    bool readyToThrow;
    public Transform lastCoinPos;
    
    private void Start()
    {
        toL = objectToThrow.GetComponent<Throwing_Obj_Logic>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(throwKey))
        {
            ManageInputState();
        }
    }

    private void ManageInputState()
    {
        if (toL.m_State == Throwing_Obj_Logic.EThrowingState.ATTACHED)
        {
            Throw();
        }
        else if(toL.m_State == Throwing_Obj_Logic.EThrowingState.THROW)
        {

        }
    }

    private void Throw()
    {
        // Dettach & Change preferences
        objectToThrow.transform.SetParent(null);
            
            // change state
        toL.SetNewState(Throwing_Obj_Logic.EThrowingState.THROW);
        
            // get rigidbody component
        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();
            // change preferences
        projectileRb.useGravity = true;
        projectileRb.isKinematic = false;
        projectileRb.interpolation = RigidbodyInterpolation.Interpolate;
        projectileRb.collisionDetectionMode = CollisionDetectionMode.Continuous;


        // Make Impulse
            // calculate direction
        Vector3 forceDirection = cam.transform.forward;

            // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
}