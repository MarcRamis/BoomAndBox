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
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashInterpTime = 3f;
    [SerializeField] private AnimationCurve dashInterpCurveSmooth;
    
    [Header("Settings")]
    [SerializeField] private bool disableGravity = false;
    [SerializeField] private LayerMask dashingLayers;
    
    [Header("Cooldown")]
    [SerializeField] private float dashCd;
    private float dashCdTimer;
    
    [Header("Inputs")]
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    
    [Header("Effects")]
    [SerializeField] private GameObject speedPs;
    [HideInInspector] public Transform m_Target;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks dashFeedback;

    // Const variablaes
    private const float targetNearDistance = 0.2f;

    // Internal variables
    private Vector3 startPosition;
    private float elapsedTime;
    
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
            // cant dash when companion is in mode: comeback, attached && throw_large
            if (tr.toL.CanDash())
            {
                // if key input dash can dash
                if (Input.GetKeyDown(dashKey))
                {
                    DoDash();
                    tr.toL.SetNewState(ThrowingObj.EThrowingState.RETAINED);
                }
                // if retained instant dash
                //else if (tr.toL.m_State == ThrowingObj.EThrowingState.RETAINED)
                //{
                //    DoDash();
                //}
                // it could dash in throw large mode if the distance is enough close like the throw mode
                //else if ()
                //{
                //
                //}
            }
            else
            {
                if (pm.isDashing)
                {
                    dashCdTimer = dashCd;
                    ResetDash();
                }
               
            }
        }

        // Reset Cooldown
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    // Fixed Update
    private void FixedUpdate()
    {
        // always this is true do the interpolation to the companion
        if (pm.isDashing)
        {
            DashInterpo();
        }
    }

    // Functions
    private void DoDash()
    {
        // cooldown
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.isDashing = true;

        // feedback
        DoEffects();
    }

    private void DashInterpo()
    {
        // change preferences
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
            ResetDash();
        }
    }
    private void ResetDash()
    {
        // change preferences again
        elapsedTime = 0;
        m_Rb.useGravity = true;
        m_Rb.isKinematic = false;
        m_Rb.interpolation = RigidbodyInterpolation.Interpolate;

        // set modes
        pm.isDashing = false;
        tr.toL.SetNewState(ThrowingObj.EThrowingState.COMEBACK);

        // gravity
        if (disableGravity)
            m_Rb.useGravity = true;
    
        // effects
        GetComponent<TrailRenderer>().emitting = false;
        speedPs.SetActive(false);
    }
    private void SelectTarget()
    {
        m_Target = tr.objectToThrow.transform;
    }
    private void DoEffects()
    {
        // start effects
        dashFeedback.PlayFeedbacks();
        speedPs.SetActive(true);
        GetComponent<TrailRenderer>().emitting = true;
    }
}