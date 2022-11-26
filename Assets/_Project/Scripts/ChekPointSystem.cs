using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChekPointSystem : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetPlayerPosToSpawn()
    {
        player.transform.position = playerSpawn.position;
    }

}

