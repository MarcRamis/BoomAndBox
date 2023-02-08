using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public enum ECompanionState
{
    NOCOMPANION,
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
    [SerializeField] private Transform modelCompanion;
    [HideInInspector] private Rigidbody m_Rb;
    [HideInInspector] private Collider m_Collider;
    [SerializeField] private GameObject prefabHitExplosion;

    [Header("Settings")]
    [SerializeField] private float maxDistanceToReturn;
    [SerializeField] private float maxLargeDistanceToReturn;
    [SerializeField] private float timeRetained;
    [HideInInspector] public ECompanionState state = ECompanionState.ATTACHED;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks comebackingFeedback;
    [SerializeField] private Color throwDashColor;
    [SerializeField] private Color throwLargeColor;
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector3 initialScale;
    private Quaternion initialRotation;



    // Start
    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        initialScale = transform.localScale;
        initialRotation = transform.localRotation;
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

                ResetInitialProperties(false);

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
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(1);
        }

        Instantiate(prefabHitExplosion, collision.GetContact(0).point, prefabHitExplosion.transform.rotation);

        if (state != ECompanionState.ATTACHED)
            state = ECompanionState.COMEBACK;
    }

    public bool CanDash()
    {
        return state != ECompanionState.ATTACHED
        && state != ECompanionState.COMEBACK
        && state != ECompanionState.THROW_LARGE;
    }
    
    public Vector3 GetPosition() { return transform.position; }

    public void ResetInitialProperties(bool changeRotation)
    {
        transform.localScale = initialScale;
        if (changeRotation) transform.localRotation = initialRotation;
    }

    public void RotateModel(Vector3 orientation)
    {
        //float rotX = Mathf.Clamp(orientation.x,-90,90);
        //float rotY = Mathf.Clamp(orientation.y,-45,45);
        //float rotZ = orientation.z;
        
        //modelCompanion.local = Vector3.Slerp(modelCompanion.forward, new Vector3(rotX, rotY, rotZ), Time.fixedDeltaTime * 50f);
    }
}       
