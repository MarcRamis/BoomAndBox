using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public enum ECompanionState
{

}

public class ThrowingObj : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private float maxDistanceToReturn;
    [SerializeField] private float maxLargeDistanceToReturn;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks comebackingFeedback;
    
    // Internal Variables
    [HideInInspector] public Vector3 startThrowingPosition;
    
    public enum EThrowingState
    {
        ATTACHED,
        THROW,
        THROW_LARGE,
        RETAINED,
        COMEBACK
    }
    [SerializeField] public EThrowingState m_State = EThrowingState.ATTACHED;
    private Rigidbody m_Rb;
    private Collider m_Collider;

    // Awake
    private void Awake()
    {
        startThrowingPosition = transform.position;
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
        if (Vector3.Distance(player.transform.position, transform.position) > maxDistanceToReturn && m_State != EThrowingState.THROW_LARGE)
        {
            if (m_State != EThrowingState.RETAINED && m_State != EThrowingState.COMEBACK)
                SetNewState(EThrowingState.RETAINED);
        }
        else if(Vector3.Distance(player.transform.position, transform.position) > maxLargeDistanceToReturn && m_State == EThrowingState.THROW_LARGE)
        {
            SetNewState(EThrowingState.COMEBACK);
        }
    }
    
    // Functions
    public void StateHandler()
    {
        switch (m_State)
        {
            case EThrowingState.ATTACHED:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = true;
                m_Rb.interpolation = RigidbodyInterpolation.None;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

                m_Collider.isTrigger = false;

                break;
            case EThrowingState.THROW:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                break;
            case EThrowingState.THROW_LARGE:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                break;
            case EThrowingState.RETAINED:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.None;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                m_Rb.velocity = Vector3.zero;

                m_Collider.isTrigger = false;

                break;
            case EThrowingState.COMEBACK:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = true;
                m_Rb.interpolation = RigidbodyInterpolation.Extrapolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                m_Collider.isTrigger = true;

                break;
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

    private void OnCollisionEnter(Collision collision)
    {
        m_State = EThrowingState.COMEBACK;
    }
    public bool CanDash()
    {
        return m_State != ThrowingObj.EThrowingState.ATTACHED
        && m_State != ThrowingObj.EThrowingState.COMEBACK
        && m_State != ThrowingObj.EThrowingState.THROW_LARGE;
    }   
}       
