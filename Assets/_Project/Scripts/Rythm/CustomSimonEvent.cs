using UnityEngine;

public class CustomSimonEvent : MonoBehaviour
{
    private Player player;
    public SimonController simonController;
    
    public delegate void OnTriggerEvent();
    public OnTriggerEvent OnTrigger;

    private void Awake()
    {
        player = ReferenceSingleton.Instance.playerScript;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetNewState(EPlayerModeState.SIMON);
            OnTrigger?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetNewState(EPlayerModeState.REGULAR);
        }
    }
}