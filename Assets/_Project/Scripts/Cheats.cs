using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//---Events
[Serializable]
public class InvencibilityEvent : UnityEvent<bool> { }
[Serializable]
public class TeleporttEvent : UnityEvent<bool> { }
//---
public class Cheats : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private KeyCode godModeKey = KeyCode.Period;
    [SerializeField] private KeyCode restartLevel = KeyCode.Comma;
    [SerializeField] private KeyCode nextCheckPoint = KeyCode.Alpha1;
    [SerializeField] private KeyCode lastCheckPoint = KeyCode.Alpha2;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("CheackPoints")]
    [SerializeField] private GameObject[] checkPoints = null;
    [SerializeField] private ChekPointSystem checkPointSystemScript = null;

    //[Header("Unity Events")]
    //[SerializeField] InvencibilityEvent Invencibility_Event;
    //[SerializeField] TeleporttEvent CheckPoint_Event;
    //[SerializeField] UnityEvent Restart_Event;

    private int currentCheackBoxIndex = 0;
    private bool godModeState = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(player == null) 
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        player.GetComponent<Player>().dashOnboarding = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(godModeKey))
            {
                player.GetComponent<Player>().SwitchGodMode();
                JSON_Creator.Instance.InvencibilityEvent(godModeState);
                //Invencibility_Event?.Invoke(godModeState);
                godModeState = !godModeState;
            }
            else if (Input.GetKeyDown(restartLevel))
            {
                //Restart_Event?.Invoke();
                JSON_Creator.Instance.PlayerDeath();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(nextCheckPoint))
            {
                if(++currentCheackBoxIndex <= checkPoints.Length - 1) 
                {
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                    checkPointSystemScript.SetPlayerPosToSpawnNoDmg();
                }
                else
                {
                    currentCheackBoxIndex = 0;
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                    checkPointSystemScript.SetPlayerPosToSpawnNoDmg();
                }
                JSON_Creator.Instance.Teleport(false);
                //CheckPoint_Event?.Invoke(false);
            }
            else if (Input.GetKeyDown(lastCheckPoint))
            {
                if (--currentCheackBoxIndex >= 0)
                {
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                    checkPointSystemScript.SetPlayerPosToSpawnNoDmg();
                }
                else
                {
                    currentCheackBoxIndex = checkPoints.Length - 1;
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                    checkPointSystemScript.SetPlayerPosToSpawnNoDmg();
                }
                JSON_Creator.Instance.Teleport(true);
                //CheckPoint_Event?.Invoke(true);
            }
        }
        
    }

    public void GiveDashToPlayer()
    {
        player.GetComponent<Player>().dashOnboarding = false;
    }

}
