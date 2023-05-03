using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Hacer da�o al enemigo
            other.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }
}
