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
public struct DeathCount
{
    public int idNumber;
    public double timeInGame;

}
[System.Serializable]
public struct LevelRestart
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
    public List<DeathCount> deathList = new List<DeathCount>();
    public List<LevelRestart> levelRestartList = new List<LevelRestart>();
    public double endTimer = 0.0f;

    private int numberOfDeaths = 0;

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
    public void DeathCountAdd(double _timeInGame) 
    {
        numberOfDeaths++;
        DeathCount _tempDeaths = new DeathCount();
        _tempDeaths.idNumber = numberOfDeaths;
        _tempDeaths.timeInGame = _timeInGame;

        deathList.Add(_tempDeaths);
    }
    public void LevelRestartAdd(int _idNumber, double _timeGame)
    {
        LevelRestart _tempLevelRestart = new LevelRestart();
        _tempLevelRestart.idNumber = _idNumber;
        _tempLevelRestart.timeInGame = _timeGame;

        levelRestartList.Add(_tempLevelRestart);
    }
}
