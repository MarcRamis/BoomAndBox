using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingSystem : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    private Rigidbody m_Rb;
    private PlayerMovementSystem pm;
    private ThrowingSystem tr;

    [Header("Dashing")]
    public float dashDistance;
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;
    private Vector3 delayedForceToApply;
    
    [Header("Settings")]
    public bool disableGravity = false;
    public bool resetVel = true;
    public LayerMask dashingLayers;
    
    // To prevent spam
    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;
    
    private Vector3 directionToDash;
    public Transform m_Target;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementSystem>();
        tr = GetComponent<ThrowingSystem>();
    }

    private void Update()
    {
        SelectTarget();

        if (m_Target != null)
        {
            // Dash to dashing obj static
            if (Input.GetKeyDown(dashKey) && m_Target.gameObject.layer == LayerMask.NameToLayer("Dashing_Obj"))
                Dash();

            // Dash to dashing obj dynamic
            if (Input.GetKeyDown(dashKey) && pm.isGrounded && tr.toL.m_State != ThrowingObj.EThrowingState.ATTACHED)
                Dash();
        }

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.isDashing = true;
        pm.maxYSpeed = maxDashYSpeed;
        
        Vector3 direction = m_Target.position - transform.position;
        direction = direction.normalized;

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;
        
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
        
        if (disableGravity)
            m_Rb.useGravity = false;

        GetComponent<TrailRenderer>().emitting = true;
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
            m_Target = null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, orientation.forward * dashDistance);
    }
}