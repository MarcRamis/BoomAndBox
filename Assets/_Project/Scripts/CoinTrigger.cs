using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;

public class CoinTrigger : MonoBehaviour
{
    [SerializeField] private CoinFeedbackController feedbackController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Companion")
        {
            EventsSystem.current.CoinCollected();
            feedbackController.PlayTakenItem();
            Destroy(gameObject);
        }
    }
}
