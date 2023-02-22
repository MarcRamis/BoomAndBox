using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DecalLogic : MonoBehaviour
{
    [SerializeField] private float distanceToWall = 0.05f;

    private float raycastLineDistance = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, raycastLineDistance))
        {
            transform.position = hit.point + (hit.normal * distanceToWall);
        }
    }
    private void OnDrawGizmos()
    {      
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastLineDistance, Color.yellow);
    }
}
