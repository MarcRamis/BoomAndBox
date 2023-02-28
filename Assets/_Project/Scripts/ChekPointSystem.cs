using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChekPointSystem : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetPlayerPosToSpawn()
    {
        player.transform.position = playerSpawn.position;
        player.GetComponent<Player>().Damage(1);
    }

    public void SetSpawnPointPos(Transform newPos)
    {
        transform.GetChild(1).position = newPos.position;
    }

}

