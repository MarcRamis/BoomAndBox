using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputController : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    [HideInInspector] private PlayerInputActions inputs;

    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public Vector2 lookDirection;

    [HideInInspector] public Action OnJumpPerformed;
    [HideInInspector] public Action OnThrowPerformed;
    [HideInInspector] public Action OnReturnPerformed;
    [HideInInspector] public Action OnDashPerformed;
    [HideInInspector] public Action OnZoomPerformed;

    private void OnEnable()
    {
        if (inputs == null)
        {
            inputs = new PlayerInputActions();
            // Tell the "Player" action map that we want to get told about
            // when actions get triggered.
            inputs.Player.SetCallbacks(this);
        }
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnThrowPerformed?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnDashPerformed?.Invoke();
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        OnReturnPerformed?.Invoke();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnZoomPerformed?.Invoke();
    }
}
