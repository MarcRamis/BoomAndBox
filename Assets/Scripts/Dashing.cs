using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    private Rigidbody rb;
    private PlayerMovement pm;
    private Throwing tr;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;
    private Vector3 delayedForceToApply;
    
    [Header("CameraEffects")]
    public ThirdPersonCam cam;
    public float dashFov;
    private float originalFov;
    
    [Header("Settings")]
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;
    
    private Vector3 directionToDash;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        tr = GetComponent<Throwing>();
        
        originalFov = cam.GetComponent<Camera>().fieldOfView;
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
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

        //cam.ChangeFov(dashFov, dashDuration);
        
        Vector3 direction = tr.lastCoinPos.position - transform.position;
        direction = direction.normalized;

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;
        
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);

        //cam.DoFov(dashFov);

        if (disableGravity)
            rb.useGravity = false;

        GetComponent<TrailRenderer>().emitting = true;
    }

    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;
    
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.isDashing = false;
        pm.maxYSpeed = 0;
        
        //cam.ChangeFov(originalFov, dashDuration);

        if (disableGravity)
            rb.useGravity = true;

        GetComponent<TrailRenderer>().emitting = false;
    }
}