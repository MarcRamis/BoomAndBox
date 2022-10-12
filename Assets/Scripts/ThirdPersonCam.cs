using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Rigidbody rb;
    
    [SerializeField] private float rotationSpeed;
   
    [SerializeField] private Transform combatLookAt;
    
    [SerializeField] private GameObject thirdPersonCam;
    [SerializeField] private GameObject combatCam;
    
    [SerializeField] private CameraStyle currentStyle;
    [SerializeField] private enum CameraStyle
    {
        Basic,
        Combat
    }
    
    [SerializeField] private ECameraType type = ECameraType.ThirdPerson;
    private enum ECameraType
    {
        FirstPerson,
        ThirdPerson
    }

    public GameObject myCurrentCamera;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (currentStyle == CameraStyle.Basic) myCurrentCamera = thirdPersonCam;
        else if (currentStyle == CameraStyle.Combat) myCurrentCamera = combatCam;
    }

    private void Update()
    {
        // switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if(currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if (newStyle == CameraStyle.Basic)
        {
            myCurrentCamera = thirdPersonCam;
            thirdPersonCam.SetActive(true);
        }

        if (newStyle == CameraStyle.Combat) 
        { 
            combatCam.SetActive(true);
            myCurrentCamera = combatCam;
        }

        currentStyle = newStyle;
    }

    public void ChangeFov(float value, float duration)
    {
        //myCurrentCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = Mathf.Lerp(, value, duration);
    }
}
