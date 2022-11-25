using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class DashingSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform orientation;
    private Rigidbody m_Rb;
    private PlayerMovementSystem pm;
    private ThrowingSystem tr;

    [Header("Dashing")]
    [SerializeField] private EDashType dashType;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpwardForce;
    [SerializeField] private float maxDashYSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashInterpTime = 3f;
    [SerializeField] private AnimationCurve dashInterpCurveSmooth;
    private Vector3 delayedForceToApply;
    
    [Header("Settings")]
    [SerializeField] private bool disableGravity = false;
    [SerializeField] private bool resetVel = true;
    [SerializeField] private LayerMask dashingLayers;
    
    // To prevent spam
    [Header("Cooldown")]
    [SerializeField] private float dashCd;
    private float dashCdTimer;
    
    [Header("Inputs")]
    [SerializeField] private KeyCode dashKey = KeyCode.E;
    
    [Header("Effects")]
    [SerializeField] private GameObject speedPs;
    [HideInInspector] public Transform m_Target;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks dashFeedback;

    // Const variablaes
    private const float targetNearDistance = 0.2f;

    // Internal variables
    private Vector3 directionToDash;
    private Vector3 startPosition;
    private float elapsedTime;
    public enum EDashType { DELAYEDFORCE, INTERPOLATION }
    
    // Start
    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementSystem>();
        tr = GetComponent<ThrowingSystem>();

        speedPs.SetActive(false);
    }

    // Update
    private void Update()
    {
        // Search for something to dash
        SelectTarget();
        
        // Dash
        if (m_Target != null)
        {
            if (Input.GetKeyDown(dashKey))
            {
                if (tr.toL.m_State != ThrowingObj.EThrowingState.ATTACHED && tr.toL.m_State != ThrowingObj.EThrowingState.COMEBACK)
                {
                    tr.toL.SetNewState(ThrowingObj.EThrowingState.RETAINED);

                    if (dashCdTimer > 0) return;
                    else dashCdTimer = dashCd;

                    pm.isDashing = true;
                    
                    DoEffects();
                }
            }
        }

        // Reset Cooldown
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (pm.isDashing)
        {
            switch (dashType)
            {
                case EDashType.DELAYEDFORCE:
                    Dash();
                    break;
                case EDashType.INTERPOLATION:
                    DashInterpo();
                    break;
            }
        }
    }

    private void DashInterpo()
    {
        // set variables
        startPosition = transform.position;
        m_Rb.useGravity = false;
        m_Rb.isKinematic = true;
        m_Rb.interpolation = RigidbodyInterpolation.Extrapolate;

        // calculate time
        elapsedTime += Time.fixedDeltaTime;
        float percentageComplete = elapsedTime / dashInterpTime;

        // make interpolation
        transform.position = Vector3.Lerp(startPosition, tr.objectToThrow.transform.position, dashInterpCurveSmooth.Evaluate(percentageComplete));

        // Calculate distance
        if (Vector3.Distance(transform.position, tr.objectToThrow.transform.position) < targetNearDistance)
        {
            // Reset
            elapsedTime = 0;
            m_Rb.useGravity = true;
            m_Rb.isKinematic = false;
            m_Rb.interpolation = RigidbodyInterpolation.Interpolate;

            ResetDash();
        }
    }


    private void Dash()
    {
        pm.maxYSpeed = maxDashYSpeed;
        
        Vector3 direction = m_Target.position - transform.position;
        direction = direction.normalized;

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;
        
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
        
        if (disableGravity)
            m_Rb.useGravity = false;
    }

    private void DelayedDashForce()
    {
        if (resetVel)
            m_Rb.velocity = Vector3.zero;
    
        m_Rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.isDashing = false;
        pm.maxYSpeed = 0;

        if (disableGravity)
            m_Rb.useGravity = true;

        GetComponent<TrailRenderer>().emitting = false;
        speedPs.SetActive(false);
    }

    private void SelectTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, orientation.forward, out hit, dashDistance, dashingLayers.value))
        {
            m_Target = hit.collider.transform;
        }
        else
        {
            m_Target = tr.objectToThrow.transform;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, orientation.forward * dashDistance);
    }

    private void DoEffects()
    {
        dashFeedback.PlayFeedbacks();
        speedPs.SetActive(true);
        GetComponent<TrailRenderer>().emitting = true;
    }
}