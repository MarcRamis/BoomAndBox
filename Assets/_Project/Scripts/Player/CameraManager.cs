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
    [SerializeField] private PlayerMovementSystem playerMovement;
    [SerializeField] private Transform followCameraTarget;
    // Reference camera in game
    [SerializeField] private GameObject thirdPersonCameraLookingAt;
    [SerializeField] private GameObject thirdPersonCameraAimLookingAt;
    [SerializeField] private GameObject thirdPersonCameraLookingAt_VirtualCamera;
    
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 2.5f;
    [SerializeField] private float rotationLerp = 0.5f;
    [HideInInspector] private ECameraStyle currentStyle;
    [HideInInspector] private Quaternion nextRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        // Esto debería ser un dispatch del player
        if (playerMovement.isAiming)
            currentStyle = ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET_AIM;
        else
            currentStyle = ECameraStyle.THIRDPERSON_LOOKING_AT_TARGET;

        HandleCamera();
    }

    private void FixedUpdate()
    {
        //Rotate the Follow Target transform based on the input
        followCameraTarget.rotation *= Quaternion.AngleAxis(playerMovement._look.x * rotationSpeed, Vector3.up);
        followCameraTarget.rotation *= Quaternion.AngleAxis(playerMovement._look.y * rotationSpeed, Vector3.right);

        var angles = followCameraTarget.localEulerAngles;
        angles.z = 0;

        var angle = followCameraTarget.localEulerAngles.x;


        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        followCameraTarget.localEulerAngles = angles;

        nextRotation = Quaternion.Lerp(followCameraTarget.rotation, nextRotation, Time.deltaTime * rotationLerp);
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