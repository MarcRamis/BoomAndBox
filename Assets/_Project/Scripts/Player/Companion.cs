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

public class Companion : MonoBehaviour
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

    [SerializeField] public ECompanionState state = ECompanionState.ATTACHED;
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
        HandleState();
    }

    // Fixed Update
    private void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > maxDistanceToReturn && state != ECompanionState.THROW_LARGE)
        {
            if (state != ECompanionState.RETAINED && state != ECompanionState.COMEBACK)
            {
                SetNewState(ECompanionState.RETAINED);
                Invoke(nameof(ResetRetainedState), timeRetained);
            }
        }
        else if(Vector3.Distance(player.transform.position, transform.position) > maxLargeDistanceToReturn && state == ECompanionState.THROW_LARGE)
        {
            SetNewState(ECompanionState.COMEBACK);
        }
    }
    
    // Functions
    private void ResetRetainedState()
    {
        SetNewState(ECompanionState.COMEBACK);
    }

    public void HandleState()
    {
        switch (state)
        {
            case ECompanionState.ATTACHED:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = true;
                m_Rb.interpolation = RigidbodyInterpolation.None;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

                m_Collider.isTrigger = true;

                trailRenderer.endColor = throwDashColor;
                trailRenderer.startColor = throwDashColor;

                break;
            case ECompanionState.THROW:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                trailRenderer.endColor = throwDashColor;
                trailRenderer.startColor = throwDashColor;

                break;
            case ECompanionState.THROW_LARGE:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.Interpolate;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                trailRenderer.endColor = throwLargeColor;
                trailRenderer.startColor = throwLargeColor;

                break;
            case ECompanionState.RETAINED:

                m_Rb.useGravity = false;
                m_Rb.isKinematic = false;
                m_Rb.interpolation = RigidbodyInterpolation.None;
                m_Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                m_Rb.velocity = Vector3.zero;

                m_Collider.isTrigger = false;

                trailRenderer.endColor = throwDashColor;
                trailRenderer.startColor = throwDashColor;

                break;
            case ECompanionState.COMEBACK:

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

    public void SetNewState(ECompanionState newState)
    {
        state = newState;
    }

    public void MakeImpulse()
    {
        m_Rb.AddForce(Vector3.forward * 5, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && state != ECompanionState.ATTACHED)
            other.gameObject.GetComponent<IDamageable>().Damage(1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && state != ECompanionState.ATTACHED)
            collision.gameObject.GetComponent<IDamageable>().Damage(1);

        if (state != ECompanionState.ATTACHED)
            state = ECompanionState.COMEBACK;
    }

    public bool CanDash()
    {
        return state != ECompanionState.ATTACHED
        && state != ECompanionState.COMEBACK
        && state != ECompanionState.THROW_LARGE;
    }   
}       
