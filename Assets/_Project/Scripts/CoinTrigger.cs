using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;

public class CoinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject pickFeedback;
    [SerializeField] private MMFeedbacks soundFeedback;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Companion")
        {
            EventsSystem.current.CoinCollected();
            Instantiate(pickFeedback, transform.position, transform.rotation);
            soundFeedback.PlayFeedbacks();
            Destroy(gameObject);
        }
    }
}
