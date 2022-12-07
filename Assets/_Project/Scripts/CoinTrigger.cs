using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Companion")
        {
            EventsSystem.current.CoinCollected();
            Destroy(gameObject);
        }
    }
}
