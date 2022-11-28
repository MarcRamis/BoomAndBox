using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private ChekPointSystem CheckPointSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            CheckPointSystem.SetSpawnPointPos(transform);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
