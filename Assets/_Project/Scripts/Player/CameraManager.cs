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

    [Header("Settings")]
    [SerializeField] private float runFov = 60;
    [SerializeField] private float timeLerpFov = 0.1f;
    [HideInInspector] private float initialFov;

    private bool fovRunningOnce = false;
    private bool fovIdleOnce = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputSystem.onDeviceChange += InputDeviceChanged;

        mainCamera.LookAt = lookAtTarget;
        initialFov = mainCamera.m_Lens.FieldOfView;
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
        if (playerMovement.isAiming)
        {
            if (mainCamera.gameObject.activeSelf) mainCamera.gameObject.SetActive(false);
            if (!mainCameraAiming.gameObject.activeSelf) mainCameraAiming.gameObject.SetActive(true);

            //if (mainCamera.m_Lens.FieldOfView == runFov)
            //{
            //    mainCamera.m_Lens.FieldOfView = initialFov;
            //}
        }

        else
        {
            if (!mainCamera.gameObject.activeSelf) mainCamera.gameObject.SetActive(true);
            if (mainCameraAiming.gameObject.activeSelf) mainCameraAiming.gameObject.SetActive(false);

            //if (playerMovement.playerRigidbody.velocity.magnitude > 0.1f)
            //{
            //    if (!fovRunningOnce)
            //    {
            //        fovRunningOnce = true;
            //        fovIdleOnce = false;
            //        StartCoroutine(LerpFieldOfView(runFov, timeLerpFov));
            //    }
            //}
            //else
            //{
            //    if (!fovIdleOnce)
            //    {
            //        fovIdleOnce = true;
            //        fovRunningOnce = false;
            //        StartCoroutine(LerpFieldOfView(initialFov, timeLerpFov));
            //    }
            //}
        }
    }

    IEnumerator LerpFieldOfView(float targetFOV, float lerpTime)
    {
        CinemachineFreeLook freeLook = mainCamera.GetComponent<CinemachineFreeLook>();

        float originalFOV = freeLook.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            freeLook.m_Lens.FieldOfView = Mathf.Lerp(originalFOV, targetFOV, elapsedTime / lerpTime);
            yield return null;
        }

        // Asegurarse de que el valor final es exactamente el targetFOV
        freeLook.m_Lens.FieldOfView = targetFOV;
    }
}