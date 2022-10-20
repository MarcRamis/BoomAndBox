using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing_Obj_Logic : MonoBehaviour
{
    public EThrowingState m_State = EThrowingState.ATTACHED;
    [SerializeField] private Transform player;
    [SerializeField] private Transform positionToRelocate;
    private BoxCollider m_Collider;
    public enum EThrowingState
    {
        ATTACHED,
        THROW,
        RETAINED,
        COMEBACK
    }
    
    [HideInInspector] public Vector3 saveVel;
    private Rigidbody m_Rb;
    
    public void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        StateHandler();
    }
    private void FixedUpdate()
    {
    }
    
    private void StateHandler()
    {
        if (m_State == EThrowingState.ATTACHED)
        {
            m_Rb.useGravity = false;
            m_Rb.isKinematic = true;
            m_Rb.interpolation = RigidbodyInterpolation.None;
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            m_Collider.enabled = false;
        }
        else if(m_State == EThrowingState.THROW)
        {
            m_Rb.useGravity = true;
            m_Rb.isKinematic = false;
            m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            m_Collider.enabled = true;
        }
        else if (m_State == EThrowingState.RETAINED)
        {
            saveVel = m_Rb.velocity;
            
            m_Rb.useGravity = false;
            m_Rb.isKinematic = false;
            m_Rb.interpolation = RigidbodyInterpolation.None;
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            m_Rb.velocity = Vector3.zero;

            m_Collider.enabled = false;
        }
        else if (m_State == EThrowingState.COMEBACK)
        {
            m_Rb.useGravity = false;
            m_Rb.isKinematic = false;
            m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
            m_Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            m_Collider.enabled = true;
        }
    }

    public void SetNewState(EThrowingState newState)
    {
        m_State = newState;
    }
}
