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
    public Throwing_Obj_Logic toL;
    
    [Header("Settings")]
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;
    
    private void Start()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(throwKey) && toL.m_State == Throwing_Obj_Logic.EThrowingState.ATTACHED)
        {
            Throw();
        }
    }
    
    private bool LookingAtThrowingObj()
    {
        return false;
    }

    private void Throw()
    {
        // Dettach the component
        objectToThrow.transform.parent = null;
        // set the throwing state of the obj
        toL.SetNewState(Throwing_Obj_Logic.EThrowingState.THROW);
        
        // Make the impulse
            // get rigidbody component
        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();
        
            // calculate direction
        Vector3 forceDirection = cam.transform.forward;

            // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
}