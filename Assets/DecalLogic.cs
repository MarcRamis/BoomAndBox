using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DecalLogic : MonoBehaviour
{
    [SerializeField] private float distanceToWall = 0.05f;
    [SerializeField] private Texture gaffiti = null;
    [SerializeField] private Color graffitiColor = Color.white;

    private float raycastLineDistance = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Renderer>().material.SetTexture("_MainTex", gaffiti);
        transform.GetComponent<Renderer>().material.SetColor("_Color", graffitiColor);

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

    private void OnValidate()
    {
        transform.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", gaffiti);
        transform.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", graffitiColor);
    }

}
