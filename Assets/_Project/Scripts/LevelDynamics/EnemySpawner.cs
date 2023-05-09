using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnerFeedbackController feedbackController;
    [Space]
    [SerializeField] private float timeIsOpen = 4.0f;
    [SerializeField] private Transform spawnPos = null;
    [SerializeField] private GameObject enemyPref;
    [SerializeField] private Transform enemyToGo;

    private bool onWait = false;
    private float time = 0.0f;
    private bool onUse = false;
    private GameObject enemy;

    private void Update()
    {
        if (onWait)
            Wait();
    }

    private void Wait()
    {
        if (time >= timeIsOpen)
        {
            onWait = false;
            time = 0.0f;
            feedbackController.PlayCloseFeedback();
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        onWait = true;
    }

    public void StartSpawnEnemy()
    {
        onUse = true;
        feedbackController.PlayMoveFeedback();
    }

    public void InstantiateEnemy()
    {
        enemy = Instantiate(enemyPref, spawnPos);
        enemy.GetComponent<AIDronManager>().seekTargets[0] = enemyToGo;
        //feedbackController.PlayEnemyMoveFeedback(); //Cambiar por un invoke que apunte a EnemySetNavMesh()
        Invoke(nameof(EnemySetNavMesh), 0.1f);
    }

    public void EnemySetNavMesh()
    {
        spawnPos.DetachChildren();
        //enemy.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void EndSpawning()
    {
        onUse = false;
    }

    public bool GetIsInUse()
    {
        return onUse;
    }

    public void ShowDestroyedSpawner()
    {
        StartCoroutine(WaitOnUse());
    }

    IEnumerator WaitOnUse()
    {      
        while (true)
        {
            yield return null;
            if(!onUse)
            {
                feedbackController.PlayDestroyedFeedback();
                break;
            }

        }
    }

}
