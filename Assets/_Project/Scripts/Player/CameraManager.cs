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

public class CameraManager : SingletonMonobehaviour<CameraManager>
{
    [Header("References")]
    [SerializeField] private PlayerMovementSystem playerMovement;
    [SerializeField] private Transform followCameraTarget;

    [SerializeField] private Transform lookAtTarget;
    
    [SerializeField] private CinemachineFreeLook mainCamera;
    [SerializeField] private CinemachineFreeLook mainCameraAiming;
    
    [HideInInspector] private Vector2 mainCameraSpeedTmp;
    [HideInInspector] private Vector2 mainCameraAimSpeedTmp;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera.LookAt = lookAtTarget;

        mainCameraSpeedTmp = new Vector2(mainCamera.m_XAxis.m_MaxSpeed, mainCamera.m_YAxis.m_MaxSpeed);
        mainCameraAimSpeedTmp = new Vector2(mainCameraAiming.m_XAxis.m_MaxSpeed, mainCameraAiming.m_YAxis.m_MaxSpeed);
    }

    public void LockCamera()
    {
        // Bloquea la cámara estableciendo las velocidades máximas de los ejes X e Y en cero.
        mainCamera.m_XAxis.m_MaxSpeed = 0;
        mainCamera.m_YAxis.m_MaxSpeed = 0;

        mainCameraAiming.m_XAxis.m_MaxSpeed = 0;
        mainCameraAiming.m_YAxis.m_MaxSpeed = 0;
    }

    public void UnlockCamera()
    {
        // Desbloquea la cámara restaurando las velocidades máximas originales de los ejes X e Y.
        // Se asignan los valores almacenados en variables temporales a las propiedades correspondientes.

        // Restaura la velocidad máxima del eje X de la cámara principal.
        mainCamera.m_XAxis.m_MaxSpeed = mainCameraSpeedTmp.x;
        // Restaura la velocidad máxima del eje Y de la cámara principal.
        mainCamera.m_YAxis.m_MaxSpeed = mainCameraSpeedTmp.y;

        // Restaura la velocidad máxima del eje X de la cámara de apuntado.
        mainCameraAiming.m_XAxis.m_MaxSpeed = mainCameraAimSpeedTmp.x;
        // Restaura la velocidad máxima del eje Y de la cámara de apuntado.
        mainCameraAiming.m_YAxis.m_MaxSpeed = mainCameraAimSpeedTmp.y;
    }
    
    public void SetAimMode()
    {
        mainCamera.gameObject.SetActive(false);
        mainCameraAiming.gameObject.SetActive(true);
    }
    
    public void SetRegularMode()
    {
        mainCamera.gameObject.SetActive(true);
        mainCameraAiming.gameObject.SetActive(false);
    }

}