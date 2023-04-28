using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnerFeedbackController feedbackController;
    [Space]
    [SerializeField] private float timeIsOpen = 4.0f;

    private bool onWait = false;
    private float time = 0.0f;
    private bool onUse = false;

    private void Update()
    {
        if(onWait)
            Wait();
    }

    private void Wait()
    {
        if(time >= timeIsOpen)
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
        InstantiateEnemy();
    }

    public void StartSpawnEnemy()
    {
        onUse = true;
        feedbackController.PlayOpenFeedback();
    }

    public void InstantiateEnemy()
    {

    }

    public void EndSpawning()
    {
        onUse = false;
    }

    public bool GetIsInUse()
    {
        return onUse;
    }

}
