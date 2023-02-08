using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class DashingSystem : MonoBehaviour
{
    [Header("Dashing")]
    [SerializeField] private float dashInterpTime = 3f;
    [SerializeField] private AnimationCurve dashInterpCurveSmooth;

    [Header("References")]
    [SerializeField] public Transform orientation;
    [HideInInspector] private Rigidbody m_Rb;
    [HideInInspector] private PlayerMovementSystem pm;
    [HideInInspector] private ThrowingSystem tr;
    [HideInInspector] private Transform currentTarget;

    [Header("Feedback")]
    [SerializeField] private GameObject speedPs;
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

        // Set companion target to dash him
        currentTarget = tr.objectToThrow.transform;

        // Initialize inputs
        pm.myInputs.OnDashPerformed += DoDash;
    }

    private void Update()
    {
        if (!tr.companion.CanDash() && pm.isDashing)
            ResetDash();
    }
    
    private void DoDash()
    {
        // Dash
        if (currentTarget != null)
        {
            // Can't dash when companion is in mode: comeback, attached
            if (tr.companion.CanDash() && !pm.isDashing)
            {
                pm.isDashing = true;
                tr.companion.SetNewState(ECompanionState.RETAINED);
                DashFeedback();
            }
        }
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
        transform.position = Vector3.Lerp(startPosition, currentTarget.transform.position, dashInterpCurveSmooth.Evaluate(percentageComplete));

        // Calculate distance
        if (Vector3.Distance(transform.position, currentTarget.transform.position) < targetNearDistance || percentageComplete >= dashInterpTime)
        {
            // Reset
            ResetDash();
        }
    }
    public void ResetDash()
    {
        // change preferences again
        elapsedTime = 0;
        m_Rb.useGravity = true;
        m_Rb.isKinematic = false;
        m_Rb.interpolation = RigidbodyInterpolation.None;

        // set modes
        pm.isDashing = false;
        tr.companion.SetNewState(ECompanionState.COMEBACK);
    
        // effects
        GetComponent<TrailRenderer>().emitting = false;
        speedPs.SetActive(false);
    }
    private void SelectTarget()
    {
        
    }
    private void DashFeedback()
    {
        // start effects
        dashFeedback.PlayFeedbacks();
        speedPs.SetActive(true);
        GetComponent<TrailRenderer>().emitting = true;
    }
}