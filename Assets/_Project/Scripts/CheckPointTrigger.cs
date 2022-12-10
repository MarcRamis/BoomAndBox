using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private ChekPointSystem CheckPointSystem;
    [SerializeField] private Transform spawnTrans;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            CheckPointSystem.SetSpawnPointPos(spawnTrans);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
