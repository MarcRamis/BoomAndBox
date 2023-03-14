using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private ChekPointSystem CheckPointSystem;
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private bool deactivateAfterUse = false;
    [SerializeField] private bool startActivated = false;

    private bool isActive = false;

    private void Start()
    {
        if(CheckPointSystem == null)
        {
            CheckPointSystem = FindObjectOfType<ChekPointSystem>();
        }

        if(startActivated)
        {
            CheckPointSystem.SetSpawnPointPos(spawnTrans);
        }

        isActive = startActivated;

        EventsSystem.current.onCheckPointActivated += OnCheckPointActivated;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !isActive)
        {
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
            isActive = false;
            Debug.Log(this.gameObject.name);
        }
            
    }

    private void OnDestroy()
    {
        EventsSystem.current.onCheckPointActivated -= OnCheckPointActivated;
    }

}
