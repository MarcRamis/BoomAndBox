using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public enum ECameraStyle
{
    THIRDPERSON_LOOKING_AT_TARGET,
    THIRDPERSON_LOOKING_AT_TARGET_AIM
}

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovementSystem playerMovement;
    [SerializeField] private Transform followCameraTarget;

    [SerializeField] private Transform lookAtTarget;
    
    [SerializeField] private CinemachineFreeLook mainCamera;
    [SerializeField] private CinemachineFreeLook mainCameraAiming;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera.LookAt = lookAtTarget;

        InputSystem.onDeviceChange += InputDeviceChanged;
    }

    private void InputDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            //New device added
            case InputDeviceChange.Added:

                break;

            //Device disconnected
            case InputDeviceChange.Disconnected:
                Debug.Log("Device disconnected");
                break;

            //Familiar device connected
            case InputDeviceChange.Reconnected:
                Debug.Log("Device reconnected");

                break;

            //Else
            default:
                break;
        }
    }
    
    private void FixedUpdate()
    {
        if (playerMovement.isAiming)
        {
            if (mainCamera.gameObject.activeSelf) mainCamera.gameObject.SetActive(false);
            if (!mainCameraAiming.gameObject.activeSelf) mainCameraAiming.gameObject.SetActive(true);
        }

        else
        {
            if (!mainCamera.gameObject.activeSelf) mainCamera.gameObject.SetActive(true);
            if (mainCameraAiming.gameObject.activeSelf) mainCameraAiming.gameObject.SetActive(false);
        }
    }

}