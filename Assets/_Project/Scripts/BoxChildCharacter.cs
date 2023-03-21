using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxChildCharacter : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        do
        {
            player = GameObject.FindGameObjectWithTag("Player");
        } while (player == null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player.transform.parent.SetParent(transform);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player.transform.parent.SetParent(transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player.transform.parent.SetParent(null);
        }
    }
}
