using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            transform.parent.GetComponent<ChekPointSystem>().SetPlayerPosToSpawn();
        }
    }
}
