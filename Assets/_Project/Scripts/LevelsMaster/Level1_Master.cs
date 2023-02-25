using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_Master : MonoBehaviour, IEvent
{
    [Header("ReceiveFrom")]
    [SerializeField] private GameObject[] listGameObjectsReceive = null;
    [Header("Companion")]
    [SerializeField] private GameObject companionEvent = null;
    [Header("Event1")]
    [SerializeField] private GameObject[] listGameObjectsEventCompanion = null;
    [Header("Event2")]
    [SerializeField] private GameObject[] listGameObjectsEvent1 = null;
    public void EventStarts()
    {

    }
    public void EventEnds()
    {

    }
    public void EventAction()
    {

    }

    public void EventAction(GameObject _gameobject)
    {
        if(_gameobject == companionEvent)
        {
            foreach (GameObject gameObject in listGameObjectsEventCompanion)
            {
                gameObject.SetActive(true);
            }
            return;
        }

        for (int i = 0; i < listGameObjectsReceive.Length; i++)
        {
            if (_gameobject == listGameObjectsReceive[i])
            {
                switch (i)
                {
                    case 0:
                        foreach (GameObject gameObject in listGameObjectsEvent1)
                        {
                            gameObject.SetActive(true);
                        }
                        break;
                    case 1:
                        
                        break;
                    
                    default:
                        // Acciones por defecto
                        break;
                }
                break;
            }
        }
    }
}
