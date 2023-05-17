using UnityEngine;

public class CustomSimonEvent : MonoBehaviour
{
    private Player player;
    public SimonController simonController;
    
    public delegate void OnTriggerEvent();
    public OnTriggerEvent OnTrigger;
    
    private bool onceTrigger;

    private void Awake()
    {
        player = ReferenceSingleton.Instance.playerScript;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!onceTrigger)
            {
                onceTrigger = true;
                Invoke(nameof(ResetOnce), 0.1f);

                player.throwingSystem.RestartCompanionPosition();
                player.SetNewState(EPlayerModeState.SIMON);
                OnTrigger?.Invoke();
            }

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!onceTrigger)
            {
                onceTrigger = true;
                Invoke(nameof(ResetOnce), 0.1f);

                player.throwingSystem.RestartCompanionPosition();
                player.SetNewState(EPlayerModeState.REGULAR);
            }
;
        }
    }

    private void ResetOnce()
    {
        onceTrigger = false;
    }
}