using UnityEngine;

public class CustomSimonEvent : MonoBehaviour
{
    private Player player;
    public SimonController simonController;

    private void Awake()
    {
        player = ReferenceSingleton.Instance.playerScript;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetNewState(EPlayerModeState.SIMON);
            simonController.PlaySimon();
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