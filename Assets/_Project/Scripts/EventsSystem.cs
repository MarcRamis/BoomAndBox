using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsSystem : MonoBehaviour
{
    //This code may be put in the custom singleton class

    public static EventsSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action onCoinCollected;
    public event Action onCheckPointActivated;
    public event Action onEnemyDeath;
    public void CoinCollected()
    {
        if(onCoinCollected != null)
        {
            onCoinCollected();
        }
    }

    public void CheckPointActivated()
    {
        if (onCheckPointActivated != null)
        {
            onCheckPointActivated();
        }
    }

    public void EnemyDeath()
    {
        if(onEnemyDeath != null)
        {
            onEnemyDeath();
        }
    }
}
