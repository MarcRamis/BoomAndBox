using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JSON_Creator : MonoBehaviour
{
    private Container container = new Container();
    private double timer = 0.0f;

    //CheeckPoint variables
    private double lastCheckPointTimer = 0.0f;
    private int lastCheckPointID = 0;

    private void Start()
    {
        container.level = SceneManager.GetActiveScene().name;
    }
    private void Update()
    {
        timer += Time.deltaTime;
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
    public void PlayerDied()
    {
        container.deathCount++;
    }
    public void CreateJSON()
    {
        container.endTimer = timer;

        string save = JsonUtility.ToJson(container);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", save);

    }

}
