using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class DronFeedbackController : AgentFeedbackController
{
    [Header("Visual Effects")]
    [SerializeField] private MMFeedbacks damageFeedback;
    [SerializeField] private MMFeedbacks dieFeedback;
    [SerializeField] public MMFeedbacks preparingForChargeFeedback;
    [SerializeField] public MMFeedbacks chargeFeedback;

    [SerializeField] public GameObject explosionPrefab;

    /////////// TAKE DAMAGE
    public override void PlayTakeDamage()
    {
        damageFeedback.PlayFeedbacks();
    }

    /////////// DIE
    public override void PlayDeath()
    {
        dieFeedback.PlayFeedbacks();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    /////////// PREPARING CHARGE
    public override void PlayPreparingCharge()
    {
        preparingForChargeFeedback.PlayFeedbacks();
    }
    
    public override void StopPreparingCharge()
    {
        preparingForChargeFeedback.StopFeedbacks();
    }

    /////////// CHARGE
    public override void PlayCharge()
    {
        chargeFeedback.PlayFeedbacks();
    }

    public override void StopCharge()
    {
        Debug.Log("entro");
        chargeFeedback.StopFeedbacks();
    }

    /////////// IMPULSE
    public override void PlayImpulse()
    {
    }
}