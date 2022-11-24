using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class Dron : Enemy
{
    [Header("References")]
    [SerializeField] private GameObject idleTarget;
    private GameObject followTarget;
    [SerializeField] private MMAutoRotate mMAutoRotate;
    [SerializeField] private MMFollowTarget mMFollowTarget;
    private Rigidbody m_Rb;


    // Start
    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        followTarget = player;
        mMFollowTarget.Target = player.transform;
        mMAutoRotate.OrbitCenterTransform = idleTarget.transform;

        ManageStates();
    }

    // Update 
    void Update()
    {
    }

    private void ManageStates()
    {
        switch (enemyState)
        {
            case EEnemyState.IDLE:
                mMFollowTarget.enabled = false;
                mMAutoRotate.enabled = true;
                break;
            case EEnemyState.FOLLOW_TARGET:
                mMAutoRotate.enabled = false;
                mMFollowTarget.enabled = true;
                break;
            case EEnemyState.CHARGING:
                break;
            case EEnemyState.CHARGE:
                break;
        }
    }
    
    private void IdlePreferences()
    {

        

    }

    private void FollowTargetPreferences()
    {

    }

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);
    }

    public override void OnDeath()
    {
        base.OnDeath();


    }
}