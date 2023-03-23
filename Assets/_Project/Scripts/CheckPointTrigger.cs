using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//---Events
[Serializable]
public class CheckPointEvent : UnityEvent<int> { }

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private int checkPointID = 0;
    [SerializeField] private ChekPointSystem CheckPointSystem;
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private GameObject checkpointFeedback;
    [SerializeField] private bool deactivateAfterUse = false;
    [SerializeField] private bool startActivated = false;

    [Header("Unity Events")]
    [SerializeField] CheckPointEvent CheckPoint_Event;

    private bool isActive = false;

    private void Start()
    {
        checkpointFeedback.SetActive(false);

        if (CheckPointSystem == null)
        {
            CheckPointSystem = FindObjectOfType<ChekPointSystem>();
        }
        
        if(startActivated)
        {
            checkpointFeedback.SetActive(true);
            CheckPointSystem.SetSpawnPointPos(spawnTrans);
        }

        isActive = startActivated;

        EventsSystem.current.onCheckPointActivated += OnCheckPointActivated;

        if(CheckPoint_Event == null)
            CheckPoint_Event = new CheckPointEvent();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !isActive)
        {
            checkpointFeedback.SetActive(true);
            CheckPointSystem.SetSpawnPointPos(spawnTrans);
            if(deactivateAfterUse)
                GetComponent<BoxCollider>().enabled = false;

            EventsSystem.current.CheckPointActivated();
            CheckPoint_Event?.Invoke(checkPointID);

            isActive = true;

        }
    }
    
    private void OnCheckPointActivated()
    {
        if(isActive)
        {
            checkpointFeedback.SetActive(false);
            isActive = false;
        }
            
    }

    private void OnDestroy()
    {
        EventsSystem.current.onCheckPointActivated -= OnCheckPointActivated;
    }

}
