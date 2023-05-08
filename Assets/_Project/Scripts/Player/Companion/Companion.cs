using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public enum ECompanionState
{
    NONE,
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
    [HideInInspector] private Rigidbody companionRigidBody;
    [HideInInspector] private Collider m_Collider;
    [SerializeField] private GameObject prefabHitExplosion;
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform socketHand;
    
    [Header("Settings")]
    [SerializeField] private float maxDistanceToReturn;
    [SerializeField] private float maxLargeDistanceToReturn;
    [SerializeField] private float timeRetained;
    [SerializeField] private bool isLevelOnboarding;
    [SerializeField] public ECompanionState state = ECompanionState.NONE;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float upDownSpeed = 1f;

    //Feedback
    [HideInInspector] public CompanionFeedbackController feedbackController;

    [HideInInspector] public bool playerAiming;
    [HideInInspector] private Vector3 initialScale;
    [HideInInspector] private Quaternion initialRotation;
    [HideInInspector] private Vector3 initialPosition;
    [HideInInspector] private Vector3 startMoveSinPosition;

    private void Awake()
    {
        companionRigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        feedbackController = GetComponent<CompanionFeedbackController>();

        initialScale = transform.localScale;
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
        startMoveSinPosition = initialPosition;
    }

    private void Start()
    {
        if (isLevelOnboarding)
        {
            SetNewState(ECompanionState.NONE);
            gameObject.SetActive(false);
        }
        else
        {
            SetNewState(ECompanionState.ATTACHED);
            gameObject.SetActive(true);
        }
    }

    // Update
    private void Update()
    {
        if (CanMoveUpIdle())
        {
            MoveUpDown();
        }
    }

    // Fixed Update
    private void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > maxDistanceToReturn && state != ECompanionState.THROW_LARGE)
        {
            if (state != ECompanionState.RETAINED && state != ECompanionState.COMEBACK)
            {
                SetNewState(ECompanionState.RETAINED);
                //startMoveSinPosition = transform.position;
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
        //startMoveSinPosition = initialPosition;
    }
    public void HandleState()
    {
        switch (state)
        {
            case ECompanionState.ATTACHED:

                companionRigidBody.useGravity = false;
                companionRigidBody.isKinematic = true;
                companionRigidBody.interpolation = RigidbodyInterpolation.None;
                companionRigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;

                m_Collider.isTrigger = true;

                feedbackController.PlayShortTrail();

                transform.parent = playerParent;

                if (!playerAiming)
                {
                    transform.SetParent(playerParent);
                    feedbackController.ShowShadow();
                }
                else
                {
                    transform.SetParent(socketHand);
                    feedbackController.HideShadow();
                }

                break;

            case ECompanionState.THROW:

                companionRigidBody.useGravity = false;
                companionRigidBody.isKinematic = false;
                companionRigidBody.interpolation = RigidbodyInterpolation.Interpolate;
                companionRigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                feedbackController.PlayShortTrail();

                feedbackController.HideShadow();

                break;
            case ECompanionState.THROW_LARGE:

                companionRigidBody.useGravity = false;
                companionRigidBody.isKinematic = false;
                companionRigidBody.interpolation = RigidbodyInterpolation.Interpolate;
                companionRigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                m_Collider.isTrigger = false;

                feedbackController.PlayLargeTrail();
                feedbackController.HideShadow();

                break;
            case ECompanionState.RETAINED:

                companionRigidBody.useGravity = false;
                companionRigidBody.isKinematic = false;
                companionRigidBody.interpolation = RigidbodyInterpolation.None;
                companionRigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                companionRigidBody.velocity = Vector3.zero;

                m_Collider.isTrigger = false;

                feedbackController.PlayShortTrail();

                ResetInitialProperties(false);

                feedbackController.ShowShadow();

                break;
            case ECompanionState.COMEBACK:

                companionRigidBody.useGravity = false;
                companionRigidBody.isKinematic = true;
                companionRigidBody.interpolation = RigidbodyInterpolation.Extrapolate;
                companionRigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                m_Collider.isTrigger = true;

                feedbackController.PlayShortTrail();

                feedbackController.ShowShadow();

                break;
        }
    }
    
    public void ApplyThrow(Vector3 forceDirection, float force)
    {
        Vector3 forceToAdd = forceDirection * force;
        companionRigidBody.AddForce(forceToAdd, ForceMode.Impulse);

        feedbackController.PlayBeingThrownFeedback();
    }

    public void SetNewState(ECompanionState newState)
    {
        state = newState;
        HandleState();
    }
    
    public void MakeImpulse()
    {
        companionRigidBody.AddForce(Vector3.forward * 5, ForceMode.Impulse);
    }

    public bool CanMoveUpIdle()
    {
        return state == ECompanionState.ATTACHED && !playerAiming;
    }

    public bool CanDash()
    {
        return state != ECompanionState.ATTACHED
        && state != ECompanionState.COMEBACK
        && state != ECompanionState.THROW_LARGE
        && state != ECompanionState.NONE;
    }

    public bool CanAim()
    {
        return state == ECompanionState.ATTACHED;
    }

    public Vector3 GetPosition() { return transform.position; }

    public void ResetInitialProperties(bool changeRotation)
    {
        transform.localScale = initialScale;
        if (changeRotation) 
            transform.localRotation = initialRotation;
    }
    public void ResetLocalPosition(Vector3 newPos)
    {
        transform.localPosition = newPos;
    }
    public void ResetPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

    private void MoveUpDown()
    {
        float newY = startMoveSinPosition.y + amplitude * Mathf.Sin(upDownSpeed * Time.time);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

    public void RotateModel(Vector3 orientation)
    {
        //float rotX = Mathf.Clamp(orientation.x,-90,90);
        //float rotY = Mathf.Clamp(orientation.y,-45,45);
        //float rotZ = orientation.z;

        //modelCompanion.local = Vector3.Slerp(modelCompanion.forward, new Vector3(rotX, rotY, rotZ), Time.fixedDeltaTime * 50f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && state != ECompanionState.ATTACHED)
            other.gameObject.GetComponent<IDamageable>().Damage(1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(1);
        }

        if (collision.gameObject.tag == "Enemy" && state != ECompanionState.ATTACHED)
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(1);
        }

        feedbackController.PlayHit(collision.GetContact(0).point);

        if (state != ECompanionState.ATTACHED)
            SetNewState(ECompanionState.COMEBACK);
    }
}       
