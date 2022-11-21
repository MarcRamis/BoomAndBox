using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObj : MonoBehaviour
{
    // Internal Variables
    private Vector3 initialPos;
    public enum EThrowingState
    {
        ATTACHED,
        THROW,
        RETAINED,
        COMEBACK
    }
    [HideInInspector] public EThrowingState m_State = EThrowingState.ATTACHED;
    private Rigidbody m_Rb;
    private Collider m_Collider;

    // Awake
    private void Awake()
    {
        initialPos = transform.position;
    }

    // Start
    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
    }

    // Update
    private void Update()
    {
        StateHandler();
    }

    // Fixed Update
    private void FixedUpdate()
    {
    }
    
    // Functions
    private void StateHandler()
    {
        if (m_State == EThrowingState.ATTACHED)
        {
            m_Rb.useGravity = false;
            m_Rb.isKinematic = true;
            m_Rb.interpolation = RigidbodyInterpolation.None;
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            m_Collider.isTrigger = false;
        }
        else if(m_State == EThrowingState.THROW)
        {
            //m_Rb.useGravity = true;
            //m_Rb.isKinematic = false;
            //m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
            //m_Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            m_Collider.isTrigger = false;
        }
        else if (m_State == EThrowingState.RETAINED)
        {
            m_Rb.useGravity = false;
            m_Rb.isKinematic = false;
            m_Rb.interpolation = RigidbodyInterpolation.None;
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            m_Rb.velocity = Vector3.zero;

            m_Collider.isTrigger = false;
        }
        else if (m_State == EThrowingState.COMEBACK)
        {
            m_Rb.useGravity = false;
            m_Rb.isKinematic = false;
            m_Rb.interpolation = RigidbodyInterpolation.Interpolate;  
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            m_Collider.isTrigger = true;
        }
    }

    public void SetNewState(EThrowingState newState)
    {
        m_State = newState;
    }

    public void MakeImpulse()
    {
        m_Rb.AddForce(Vector3.forward * 5, ForceMode.Impulse);
    }
    private void EnableDashCollider()
    {

    }
}
