using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCombatEvent : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = ReferenceSingleton.Instance.playerScript;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetNewState(EPlayerModeState.COMBAT);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetNewState(EPlayerModeState.REGULAR);
        }
    }
}
