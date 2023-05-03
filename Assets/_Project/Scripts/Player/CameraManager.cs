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

    private Vector2 mainCameraSpeedTmp;
    private Vector2 mainCameraAimSpeedTmp;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera.LookAt = lookAtTarget;

        mainCameraSpeedTmp = new Vector2(mainCamera.m_XAxis.m_MaxSpeed, mainCamera.m_YAxis.m_MaxSpeed);
        mainCameraAimSpeedTmp = new Vector2(mainCameraAiming.m_XAxis.m_MaxSpeed, mainCameraAiming.m_YAxis.m_MaxSpeed);
    }

    private void Update()
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
    
    public void LockCamera()
    {
        mainCamera.m_XAxis.m_MaxSpeed = 0;
        mainCamera.m_YAxis.m_MaxSpeed = 0;

        mainCameraAiming.m_XAxis.m_MaxSpeed = 0;
        mainCameraAiming.m_YAxis.m_MaxSpeed = 0;
    }
    
    public void UnlockCamera()
    {
        mainCamera.m_XAxis.m_MaxSpeed = mainCameraSpeedTmp.x;
        mainCamera.m_YAxis.m_MaxSpeed = mainCameraSpeedTmp.y;

        mainCameraAiming.m_XAxis.m_MaxSpeed = mainCameraAimSpeedTmp.x;
        mainCameraAiming.m_YAxis.m_MaxSpeed = mainCameraAimSpeedTmp.y;
    }
}