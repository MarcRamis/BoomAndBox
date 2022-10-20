using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public Transform standPosition;
    public Transform toAttach;
    public GameObject objectToThrow;
    private Throwing_Obj_Logic toL;
    
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
    
    private void Start()
    {
        toL = objectToThrow.GetComponent<Throwing_Obj_Logic>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(throwKey))
        {
            if (toL.m_State == Throwing_Obj_Logic.EThrowingState.ATTACHED)
            {
                objectToThrow.transform.SetParent(null);
                Throw();
            }
            else if(toL.m_State == Throwing_Obj_Logic.EThrowingState.THROW)
            {
                toL.SetNewState(Throwing_Obj_Logic.EThrowingState.RETAINED);
            }
        }
        if (Input.GetKeyUp(throwKey) && toL.m_State == Throwing_Obj_Logic.EThrowingState.RETAINED)
        {
            Throw();
        }
        
        if (Input.GetKeyDown(returnKey) && toL.m_State != Throwing_Obj_Logic.EThrowingState.ATTACHED)
        {
            toL.SetNewState(Throwing_Obj_Logic.EThrowingState.COMEBACK);
        }
    }

    private void FixedUpdate()
    {
        if (toL.m_State != Throwing_Obj_Logic.EThrowingState.COMEBACK) return;
            
        ComeBack();
    }

    private void Throw()
    {
        // Preferences
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
            toL.SetNewState(Throwing_Obj_Logic.EThrowingState.ATTACHED);
            objectToThrow.transform.SetParent(toAttach);
        }
    }
}