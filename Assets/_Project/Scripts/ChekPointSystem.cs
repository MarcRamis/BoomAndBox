using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChekPointSystem : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject player;
    [SerializeField] private Player playerScript;

    private void Awake()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        playerScript = player.GetComponent<Player>();
    }

    public void SetPlayerPosToSpawn()
    {
        player.transform.position = playerSpawn.position;
        playerScript.Damage(1);
        //playerScript.BlockInputsToAllow();

    }
    public void SetPlayerPosToSpawnNoDmg()
    {
        player.transform.position = playerSpawn.position;
    }

    public void SetSpawnPointPos(Transform newPos)
    {
        transform.GetChild(1).position = newPos.position;
    }
}

