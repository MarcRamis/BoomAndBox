using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing_Obj_Logic : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(BlockGravity), 0.5f);

        Invoke(nameof(DestroyItself), 10.0f);
    }

    private void BlockGravity()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;
    }

    private void DestroyItself()
    {
        Destroy(this.gameObject);
    }
}
