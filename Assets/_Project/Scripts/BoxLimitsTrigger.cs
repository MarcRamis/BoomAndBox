using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLimitsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            transform.parent.GetComponent<ChekPointSystem>().SetPlayerPosToSpawn();
        }
        else if(other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<IDamageable>().Damage(99);
        }
    }
}
