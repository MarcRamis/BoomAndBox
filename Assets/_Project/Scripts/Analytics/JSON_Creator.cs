using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JSON_Creator : MonoBehaviour
{
    // Static
    public static JSON_Creator Instance;

    private string levelName;

    private Container container = new Container();
    private double timer = 0.0f;

    //CheeckPoint variables
    private double lastCheckPointTimer = 0.0f;
    private int lastCheckPointID = 0;
    private int numberDeaths = 0;

    private void Awake()
    {
        if (JSON_Creator.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        levelName = SceneManager.GetActiveScene().name;

        if (container.level != null && levelName != container.level)
        {
            ResetClass();
        }
        else
        {
            container.level = levelName;
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void ResetClass()
    {
        container = new Container();
        timer = 0.0f;

        //CheeckPoint variables
        lastCheckPointTimer = 0.0f;
        lastCheckPointID = 0;
        numberDeaths = 0;
    }
    public void InvencibilityEvent(bool _state)
    {
        container.InvencibilityAdd(_state, timer);
    }
    public void Teleport(bool _state)
    {
        container.TeleportAdd(_state, timer);
    }
    public void CheckPoint(int _toID)
    {
        container.CheckPointAdd(lastCheckPointID, _toID, lastCheckPointTimer, timer);

        lastCheckPointID = _toID;
        lastCheckPointTimer = timer;

    }
    public void Puzzle(int _puzzleID, int _puzzleElementOrder)
    {
        container.PuzzleAdd(_puzzleID, _puzzleElementOrder, timer);
    }
    public void PlayerGetsHit()
    {
        container.HitCountAdd(timer);
    }
    public void PlayerDeath()
    {
        container.PlayerDied(timer);
    }
    public void PlayerFall()
    {
        container.PlayerFell(timer);
    }
    public void CreateJSON()
    {
        container.endTimer = timer;

        string save = JsonUtility.ToJson(container);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", save);

    }

}
