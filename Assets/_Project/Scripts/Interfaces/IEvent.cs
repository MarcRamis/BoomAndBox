using UnityEngine;

public interface IEvent
{
    void EventStarts();
    void EventEnds();
    void EventAction();
    void EventAction(GameObject _gameObject);
}
