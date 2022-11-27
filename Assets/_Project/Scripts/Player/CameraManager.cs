using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public enum ECameraStyle
{
    THIRDPERSON_LOOKING_AT_TARGET,
    THIRDPERSON_LOOKING_AT_TARGET_AIM
}

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform combatLookAt;
    
    // Reference camera in game
    [SerializeField] private GameObject thirdPersonCameraLookingAt;
    [SerializeField] private GameObject thirdPersonCameraAimLookingAt;

    [Header("Settings")]
    [SerializeField] private float rotationObjSpeed;
    private ECameraStyle currentStyle;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (player.GetComponent<PlayerMovementSystem>().isAiming)
            currentStyle = ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET_AIM;
        else
            currentStyle = ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET;

        HandleCamera();
    }

    private void FixedUpdate()
    {
        // rotate orientation
        Vector3 viewDir = player.transform.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        
        if (currentStyle == ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET || currentStyle == ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET_AIM)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationObjSpeed);
        }
    }
    private void HandleCamera()
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
