using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class Dron : Enemy
{
    /// <summary>
    /// How the Dron works!
    /// 
    /// IDLE -> CHASE PLAYER -> IDLE
    /// </summary>

    // References
    [Header("Feedback")]
    [SerializeField] private MMFeedbacks damageFeedback;
    [SerializeField] private MMFeedbacks dieFeedback;
    [SerializeField] private MMFeedbacks hitPlayerFeedback;
    [SerializeField] public GameObject chargePrefab;
    [SerializeField] public GameObject explosionPrefab;
    [SerializeField] public GameObject model;

    // Constant variables

    // Internal variables

    // Start

    private void Start()
    {
        base.Start();
    }

    // Update 
    void Update()
    {
        base.Update();
    }
    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);

        damageFeedback.PlayFeedbacks();
    }

    public override void OnDeath()
    {
        base.OnDeath();

        dieFeedback.PlayFeedbacks();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hitPlayerFeedback.PlayFeedbacks();
            collision.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }
}