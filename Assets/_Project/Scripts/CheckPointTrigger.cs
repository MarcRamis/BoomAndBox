using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private ChekPointSystem CheckPointSystem;
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private GameObject checkpointFeedback;
    [SerializeField] private bool deactivateAfterUse = false;
    [SerializeField] private bool startActivated = false;

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
