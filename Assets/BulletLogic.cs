using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10.0f;

    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = (transform.rotation * Vector3.right) * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().Damage(1);
        }
        Debug.Log(other.gameObject);
        if(!other.isTrigger)
        {
            Destroy(this.gameObject);
        }
    }
}
