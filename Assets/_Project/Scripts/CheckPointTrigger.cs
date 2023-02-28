using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private ChekPointSystem CheckPointSystem;
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private bool deactivateAfterUse = false;
    [SerializeField] private bool startActivated = false;

    private void Start()
    {
        if(CheckPointSystem == null)
        {
            CheckPointSystem = FindObjectOfType<ChekPointSystem>();
        }

        if(startActivated)
        {
            CheckPointSystem.SetSpawnPointPos(spawnTrans);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            CheckPointSystem.SetSpawnPointPos(spawnTrans);
            if(deactivateAfterUse)
                GetComponent<BoxCollider>().enabled = false;
        }
    }
}
