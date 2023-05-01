using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Windows;

public class MenuInputController : MonoBehaviour, PlayerInputActions.IUIActions
{
    // --- Variables
    [HideInInspector] public PlayerInputActions inputsUI;

    // --- Actions
    [HideInInspector] public Action onPauseInput;
    [HideInInspector] public Action onContinueTextInput;


    private InputAction escapeMenu;

    // Start is called before the first frame update
    void Awake()
    {
        if (inputsUI == null)
        {
            inputsUI = new PlayerInputActions();
            // Tell the "Player" action map that we want to get told about
            // when actions get triggered.
            inputsUI.UI.SetCallbacks(this);
        }
        inputsUI.UI.Enable();

    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        onPauseInput?.Invoke();
    }

    public void OnContinueText(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        onContinueTextInput?.Invoke();
    }

    private void OnDisable()
    {
        inputsUI.UI.Disable();
    }

    private void OnDestroy()
    {
        inputsUI.UI.Disable();
    }
}
