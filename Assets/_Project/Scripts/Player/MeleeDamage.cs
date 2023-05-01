using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Hacer daño al enemigo
            other.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }
}
