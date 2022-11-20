using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform combatLookAt;
    
    // Reference camera in game
    [SerializeField] private GameObject newCamera;
    
    [Header("Settings")]
    [SerializeField] private float rotationObjSpeed;
    [SerializeField] private CameraStyle currentStyle;
    [SerializeField] private ECameraType type = ECameraType.ThirdPerson;

    private enum CameraStyle
    {
        New_Camera
    }
    
    private enum ECameraType
    {
        ThirdPerson
    }
    [HideInInspector] public CinemachineFreeLook myCurrentCamera;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        
        if (currentStyle == CameraStyle.New_Camera)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationObjSpeed);
        }
    }
}
