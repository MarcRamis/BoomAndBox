using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class DronFeedbackController : AgentFeedbackController
{
    [Space]
    [Header("Visual Effects")]
    [SerializeField] private MMFeedbacks damageFeedback;
    [SerializeField] private MMFeedbacks dieFeedback;
    [SerializeField] private MMFeedbacks preparingForChargeFeedback;
    [SerializeField] private MMFeedbacks chargeFeedback;
    [SerializeField] private MMFeedbacks walkFeedback;
    [SerializeField] private MMFeedbacks runFeedback;
    [Space]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject dronChargingPrefab;
    [SerializeField] private GameObject receiveHitPrefab;
    [Space]
    [SerializeField] private TrailRenderer trail1;
    [SerializeField] private TrailRenderer trail2;
    [SerializeField] private TrailRenderer trail3;
    [Space]
    [SerializeField] private MeshRenderer[] mainMR;
    [SerializeField] private Material materialWF;
    [SerializeField] private Material materialYF;
    [SerializeField] private Material materialOF;
    [Space]
    [SerializeField] private Transform hitPosition;

    private void Awake()
    {
        walkFeedback.Initialization();
        runFeedback.Initialization();
    }

    /////////// TAKE DAMAGE
    public override void PlayTakeDamage()
    {
        damageFeedback.PlayFeedbacks();
        Instantiate(receiveHitPrefab, hitPosition.position, hitPosition.rotation);
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
        dronChargingPrefab.SetActive(true);
        //preparingForChargeFeedback.PlayFeedbacks();
    }
    
    public override void StopPreparingCharge()
    {
        dronChargingPrefab.SetActive(false);
        //preparingForChargeFeedback.StopFeedbacks();
    }

    /////////// CHARGE
    public override void PlayCharge()
    {
        trail1.emitting = true;
        trail2.emitting = true;
        trail3.emitting = true;
        chargeFeedback.PlayFeedbacks();
    }

    public override void StopCharge()
    {
        trail1.emitting = false;
        trail2.emitting = false;
        trail3.emitting = false;
        chargeFeedback.StopFeedbacks();
    }

    /////////// IMPULSE
    public override void PlayImpulse()
    {
    }
    
    /////////// IMPULSE
    public override void PlayWalk()
    {
        walkFeedback.PlayFeedbacks();
    }
    /////////// RUN
    public override void PlayRun()
    {
        runFeedback.PlayFeedbacks();
    }

    /////////// NEARLY DEATH
    public override void PlayNearlyDeath()
    {
        ChangeMaterial(materialOF);
    }

    /////////// MIDDLE DEATH
    public override void PlayMiddleDeath()
    {
        ChangeMaterial(materialYF);
    }

    private void ChangeMaterial(Material m)
    {
        foreach (MeshRenderer mr in mainMR)
        {
            mr.material = m;
        }
    }
}