using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatEvent : MonoBehaviour
{
    [System.Serializable] public struct EnemyRound
    {
        public int numberEnemies;

    }


    public Player player;

    public Enemy[] enemy;

    public EnemyRound[] enemyRounds;

    public GameObject[] enemySpawners;

    [Header("Events")]
    [SerializeField] private UnityEvent EndCombatEvent;

    private bool combatZoneActivated = false;
    private int currentCombatRound = 0;
    private int enemiesDefeated = 0;

    private void Start()
    {
        if(EventsSystem.current != null)
            EventsSystem.current.onEnemyDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath()
    {
        if(combatZoneActivated)
        {
            enemiesDefeated++;

            if (enemyRounds[currentCombatRound - 1].numberEnemies <= enemiesDefeated)
            {
                if(currentCombatRound > enemyRounds.Length - 1)
                {
                    //End combat zone event
                    CombatZoneEnd();
                }
                else
                {
                    enemiesDefeated = 0;
                    StartRound();
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetNewState(EPlayerModeState.COMBAT);
            combatZoneActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetNewState(EPlayerModeState.REGULAR);
        }
    }

    public void StartRound()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void CombatZoneEnd()
    {
        combatZoneActivated = false;
        player.SetNewState(EPlayerModeState.REGULAR);
        enemySpawners[0].GetComponent<EnemySpawner>().ShowDestroyedSpawner();
        EndCombatEvent?.Invoke();
        this.gameObject.SetActive(false);
    }

    IEnumerator SpawnEnemies()
    {
        int currentEnemiesSpawned = 0;
        currentCombatRound++;
        while (currentEnemiesSpawned < enemyRounds[currentCombatRound - 1].numberEnemies)
        {
            yield return null;
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                if(currentEnemiesSpawned >= enemyRounds[currentCombatRound - 1].numberEnemies)
                {
                    break;
                }

                EnemySpawner tempSpawner = enemySpawners[i].GetComponent<EnemySpawner>();

                if (!tempSpawner.GetIsInUse())
                {
                    tempSpawner.StartSpawnEnemy();
                    currentEnemiesSpawned++;
                }
            }
        }
    }

}
