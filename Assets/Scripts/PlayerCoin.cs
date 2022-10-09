using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
    [Header("Impulse")]
    [SerializeField] private Transform direction;
    [SerializeField] private float rayGoInDistance = 20.0f;
    [SerializeField] private float rayGoOutDistance = 20.0f;
    [SerializeField] private float impulseForce = 10.0f;
    [SerializeField] private float goInCooldown;
    [SerializeField] private float goOutCooldown;
    [SerializeField] private bool canGoIn;
    [SerializeField] private bool canGoOut;

    [Header("Keybinds")]
    [SerializeField] private KeyCode goInKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode goOutKey = KeyCode.Mouse1;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        canGoIn = true;
        canGoOut = true;
    }

    private void Update()
    {
        // Impulse to destination
        if (Input.GetKey(goInKey) && canGoIn)
        {
            if (MakeRaycast(rayGoInDistance))
            {
                Impulse(direction.forward.normalized);
            }

            canGoIn = false;
            Invoke(nameof(ResetGoIn), goInCooldown);
        }

        // Reverse impulse
        else if (Input.GetKey(goOutKey) && canGoOut)
        {
            if (MakeRaycast(rayGoOutDistance))
            {
                Impulse(direction.forward.normalized * -1);
            }
            canGoOut = false;
            Invoke(nameof(ResetGoOut), goOutCooldown);
        }
    }

    private bool MakeRaycast(float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.forward, out hit, distance))
        {
            return true;
        }
        return false;
    }

    private void Impulse(Vector3 _dirImpulse)
    {
        rb.AddForce(_dirImpulse * impulseForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction.forward * rayGoInDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, direction.forward * rayGoOutDistance);
    }

    private void ResetGoIn()
    {
        canGoIn = true;
    }

    private void ResetGoOut()
    {
        canGoOut = true;
    }
}