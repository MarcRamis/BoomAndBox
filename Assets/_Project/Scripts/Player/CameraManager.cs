using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
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

    // Reference camera in game
    [SerializeField] private GameObject thirdPersonCameraLookingAt;
    [SerializeField] private GameObject thirdPersonCameraAimLookingAt;
    [SerializeField] private CinemachineFreeLook m_camera;

    [Header("Settings")]
    [SerializeField] private Vector2 speedWMouse = new Vector2(0.003f, 0.2f);
    [SerializeField] private Vector2 speedWController = new Vector2(0.05f, 3f);
    [HideInInspector] private ECameraStyle currentStyle;

    bool x = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_camera.LookAt = lookAtTarget;

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

    private void Update()
    {
        // Esto debería ser un dispatch del player
        if (playerMovement.isAiming)
            currentStyle = ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET_AIM;
        else
            currentStyle = ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET;

        HandleCamera();

        //ar x = PlayerInput.currentControlScheme;
    }

    private void FixedUpdate()
    {
    }

    private void HandleCamera()
    {
        switch (currentStyle)
        {
            case ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET:

                break;

            case ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET_AIM:

                break;
        }
    }

    private void HandleCamera2()
    {
        switch (currentStyle)
        {
            case ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET:

                // Im doing this because the looking at values are the previous used in the camera mode,
                // so i want the camera to be the same that the last camera
                thirdPersonCameraAimLookingAt.transform.position = thirdPersonCameraLookingAt.transform.position;
                thirdPersonCameraAimLookingAt.GetComponent<CinemachineFreeLook>().m_XAxis.Value = thirdPersonCameraLookingAt.GetComponent<CinemachineFreeLook>().m_XAxis.Value;
                thirdPersonCameraAimLookingAt.GetComponent<CinemachineFreeLook>().m_YAxis.Value = thirdPersonCameraLookingAt.GetComponent<CinemachineFreeLook>().m_YAxis.Value;

                thirdPersonCameraAimLookingAt.SetActive(false);
                thirdPersonCameraLookingAt.SetActive(true);
                
                break;
                
            case ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET_AIM:

                thirdPersonCameraLookingAt.transform.position = thirdPersonCameraAimLookingAt.transform.position;
                thirdPersonCameraLookingAt.GetComponent<CinemachineFreeLook>().m_XAxis.Value = thirdPersonCameraAimLookingAt.GetComponent<CinemachineFreeLook>().m_XAxis.Value;
                thirdPersonCameraLookingAt.GetComponent<CinemachineFreeLook>().m_YAxis.Value = thirdPersonCameraAimLookingAt.GetComponent<CinemachineFreeLook>().m_YAxis.Value;

                thirdPersonCameraAimLookingAt.SetActive(true);
                thirdPersonCameraLookingAt.SetActive(false);

                break;
        }
    }
}