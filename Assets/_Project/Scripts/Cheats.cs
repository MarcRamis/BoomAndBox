using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private int currentCheackBoxIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if(player == null) 
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            if (Input.GetKeyDown(godModeKey))
            {
                player.GetComponent<Player>().SwitchGodMode();
            }
            else if (Input.GetKeyDown(restartLevel))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(nextCheckPoint))
            {
                if(++currentCheackBoxIndex <= checkPoints.Length - 1) 
                {
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                }
                else
                {
                    currentCheackBoxIndex = 0;
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                }
            }
            else if (Input.GetKeyDown(lastCheckPoint))
            {
                if (--currentCheackBoxIndex >= 0)
                {
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                }
                else
                {
                    currentCheackBoxIndex = checkPoints.Length - 1;
                    checkPointSystemScript.SetSpawnPointPos(checkPoints[currentCheackBoxIndex].transform);
                }
            }
        }
        
    }
}
