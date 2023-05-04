using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Puzzle;

[System.Serializable]
public struct Invencibility
{
    public enum CheatState
    {
        On,
        Off
    };

    public CheatState state;
    public double timeInGame;
    
}
[System.Serializable]
public struct Teleport
{
    public enum CheatState
    {
        Back,
        Next
    };

    public CheatState state;
    public double timeInGame;

}
[System.Serializable]
public struct CheckPoints
{
    public int fromTeleport;
    public int toTeleport;
    public double timeInGameStart;
    public double timeInGameEnd;
}
[System.Serializable]
public struct Puzzle
{
    public int id;
    [System.Serializable]
    public struct Elements
    {
        public int puzzleElementID;
        public double timeInGame;
    }
    public List<Elements> elements;

}
[System.Serializable]
public struct HitCount
{
    public int idNumber;
    public double timeInGame;

}
[System.Serializable]
public struct PlayerDeaths
{
    public int idNumber;
    public double timeInGame; 

}
[System.Serializable]
public struct PlayerFall
{
    public int idNumber;
    public double timeInGame;

}
[System.Serializable]
public class Container
{
    public string level;
    public List<Invencibility> invencibilityList = new List<Invencibility>();
    public List<Teleport> teleports = new List<Teleport>();
    public List<CheckPoints> checkPointsList = new List<CheckPoints>();
    public List<Puzzle> puzzleList = new List<Puzzle>();
    public List<HitCount> hitList = new List<HitCount>();
    public List<PlayerDeaths> levelDeathsList = new List<PlayerDeaths>();
    public List<PlayerFall> playerFallsList = new List<PlayerFall>();
    public double endTimer = 0.0f;

    private int numberOfHits = 0;

    private int numberOfDeaths = 0;

    private int numberOfFalls = 0;

    public void InvencibilityAdd(bool _state, double _timeInGame)
    {
        Invencibility _tempInvencibility = new Invencibility();
        if (_state)
            _tempInvencibility.state = Invencibility.CheatState.On;
        else
            _tempInvencibility.state = Invencibility.CheatState.Off;

        _tempInvencibility.timeInGame = _timeInGame;

        invencibilityList.Add(_tempInvencibility);
    }
    public void TeleportAdd(bool _state, double _timeInGame)
    {
        Teleport _tempTeleport = new Teleport();
        if (_state)
            _tempTeleport.state = Teleport.CheatState.Back;
        else
            _tempTeleport.state = Teleport.CheatState.Next;

        _tempTeleport.timeInGame = _timeInGame;

        teleports.Add(_tempTeleport);
    }
    public void CheckPointAdd(int _fromID, int _toID, double _startTime, double _endTime)
    {
        CheckPoints _tempCheckPoint = new CheckPoints();
        _tempCheckPoint.fromTeleport = _fromID;
        _tempCheckPoint.toTeleport = _toID;
        _tempCheckPoint.timeInGameStart = _startTime;
        _tempCheckPoint.timeInGameEnd = _endTime;

        checkPointsList.Add(_tempCheckPoint);
    }
    public void PuzzleAdd(int _puzzleID, int _puzzleElementOrder, double _timeInGame)
    {
        Puzzle _tempPuzzle = new Puzzle();
        _tempPuzzle.elements = new List<Elements>();
        Elements _tempElements = new Elements();

        foreach (var puzzle in puzzleList)
        {
            if (puzzle.id == _puzzleID)
            {
                _tempElements.puzzleElementID = _puzzleElementOrder;
                _tempElements.timeInGame = _timeInGame;

                puzzle.elements.Add(_tempElements);
                return;
            }

        }
        _tempPuzzle.id = _puzzleID;
        _tempElements.puzzleElementID = _puzzleElementOrder;
        _tempElements.timeInGame = _timeInGame;
        _tempPuzzle.elements.Add(_tempElements);

        puzzleList.Add(_tempPuzzle);
    }
    public void HitCountAdd(double _timeInGame) 
    {
        numberOfHits++;
        HitCount _tempHits = new HitCount();
        _tempHits.idNumber = numberOfHits;
        _tempHits.timeInGame = _timeInGame;

        hitList.Add(_tempHits);
    }
    public void PlayerDied(double _timeGame)
    {
        numberOfDeaths++;
        PlayerDeaths _tempLevelRestart = new PlayerDeaths();
        _tempLevelRestart.idNumber = numberOfDeaths;
        _tempLevelRestart.timeInGame = _timeGame;

        levelDeathsList.Add(_tempLevelRestart);
    }
    public void PlayerFell(double _timeGame)
    {
        numberOfFalls++;
        PlayerFall _tempLevelRestart = new PlayerFall();
        _tempLevelRestart.idNumber = numberOfFalls;
        _tempLevelRestart.timeInGame = _timeGame;

        playerFallsList.Add(_tempLevelRestart);
    }
}
