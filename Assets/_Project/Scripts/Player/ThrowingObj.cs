using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public enum ECompanionState
{
    ATTACHED,
    THROW,
    THROW_LARGE,
    RETAINED,
    COMEBACK
}

public class ThrowingObj : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private float maxDistanceToReturn;
    [SerializeField] private float maxLargeDistanceToReturn;
    [SerializeField] private float timeRetained;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks comebackingFeedback;
    [SerializeField] private Color throwDashColor;
    [SerializeField] private Color throwLargeColor;

    // Internal Variables
    [HideInInspector] public Vector3 startThrowingPosition;
    [HideInInspector] private TrailRenderer trailRenderer;

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
        trailRenderer = GetComponent<TrailRenderer>();
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
            {
                SetNewState(EThrowingState.RETAINED);
                Invoke(nameof(ResetRetainedState), timeRetained);
            }
        }
        else if(Vector3.Distance(player.transform.position, transform.position) > maxLargeDistanceToReturn && m_State == EThrowingState.THROW_LARGE)
        {
            SetNewState(EThrowingState.COMEBACK);
        }
    }
    
    // Functions
    private void ResetRetainedState()
    {
        SetNewState(EThrowingState.COMEBACK);
    }

    public void StateHandler()
    {
        switch (m_State)
        {
            case EThrowingState.ATTACHED:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = true;
                m_Rb.interpolation = RigidbodyInterpolation.None;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

                m_Collider.isTrigger = true;

                trailRenderer.endColor = throwDashColor;
                trailRenderer.startColor = throwDashColor;

                break;
            case EThrowingState.THROW:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                trailRenderer.endColor = throwDashColor;
                trailRenderer.startColor = throwDashColor;

                break;
            case EThrowingState.THROW_LARGE:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                trailRenderer.endColor = throwLargeColor;
                trailRenderer.startColor = throwLargeColor;

                break;
            case EThrowingState.RETAINED:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.None;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                m_Rb.velocity = Vector3.zero;

                m_Collider.isTrigger = false;

                trailRenderer.endColor = throwDashColor;
                trailRenderer.startColor = throwDashColor;

                break;
            case EThrowingState.COMEBACK:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = true;
                m_Rb.interpolation = RigidbodyInterpolation.Extrapolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                m_Collider.isTrigger = true;

                trailRenderer.endColor = throwDashColor;
                trailRenderer.startColor = throwDashColor;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && m_State != EThrowingState.ATTACHED)
            other.gameObject.GetComponent<IDamageable>().Damage(1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && m_State != EThrowingState.ATTACHED)
            collision.gameObject.GetComponent<IDamageable>().Damage(1);

        if (m_State != EThrowingState.ATTACHED)
            m_State = EThrowingState.COMEBACK;
    }

    public bool CanDash()
    {
        return m_State != ThrowingObj.EThrowingState.ATTACHED
        && m_State != ThrowingObj.EThrowingState.COMEBACK
        && m_State != ThrowingObj.EThrowingState.THROW_LARGE;
    }   
}       
