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
    
    // To prevent spam
    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;
    
    private Vector3 directionToDash;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementSystem>();
        tr = GetComponent<ThrowingSystem>();
    }

    private void Update()
    {
        MakeRaycast(dashDistance);

        if (Input.GetKeyDown(dashKey) && pm.isGrounded)
            Dash();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.isDashing = true;
        pm.maxYSpeed = maxDashYSpeed;
        
        Vector3 direction = tr.objectToThrow.transform.position - transform.position;
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

    private void MakeRaycast(float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, orientation.forward, out hit, distance))
        {
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, orientation.forward * dashDistance);
    }
}