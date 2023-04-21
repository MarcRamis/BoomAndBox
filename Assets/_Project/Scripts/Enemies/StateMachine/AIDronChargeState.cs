using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDronChargeState : IAIState
{
    // Internal variables
    private MTimer chargeTimer;
    private MTimer preparingChargeTimer;

    private bool activatePhysics = false;
    private bool doCharge = false;
    private float preparingChargeTime = 5f;
    private float nearlyCharge;
    
    private const float nearlyChargePecentage = 0.6f;
    private const float chargeTime = 0.5f;
    private const float sumPreparingChargeTime = 0.5f;
    
    public void Enter(Agent agent)
    {
        chargeTimer = new MTimer();
        chargeTimer.OnTimerEnd += ChargeFinished;
        
        preparingChargeTimer = new MTimer();
        preparingChargeTimer.SetTimeLimit(preparingChargeTime);
        preparingChargeTimer.OnTimerEnd += PreparingChargeFinished;
        preparingChargeTimer.StartTimer();

        nearlyCharge = preparingChargeTime * nearlyChargePecentage;
        
        DesactivatePhysiscs(agent);
    }
    
    public void Exit(Agent agent)
    {
        agent.feedbackController.StopPreparingCharge();
        agent.feedbackController.StopCharge();
 
        ActivatePhysics(agent);

        agent.playerScript.beingTargettedBy = null;
    }

    public EAIState GetId()
    {
        return EAIState.CHARGE;
    }
    
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);
        chargeTimer.Update(Time.fixedDeltaTime);
        preparingChargeTimer.Update(Time.fixedDeltaTime);

        CalculateNearlyCharge(agent);

        if (doCharge)
        {
            agent.feedbackController.PlayCharge();
            doCharge = false;
            DesactivatePhysiscs(agent);
            Charge(agent);

            chargeTimer.SetTimeLimit(chargeTime);
            chargeTimer.StartTimer();
        }
        
        if (activatePhysics)
        {
            agent.feedbackController.StopCharge();
            activatePhysics = false;
            ActivatePhysics(agent);
            
            preparingChargeTime += sumPreparingChargeTime;
            preparingChargeTimer.SetTimeLimit(preparingChargeTime);
            preparingChargeTimer.StartTimer();
        }

        LookAtPlayer(agent);
    }

    private void LookAtPlayer(Agent agent)
    {
        Vector3 lockY = new Vector3(agent.player.transform.position.x, agent.player.transform.position.y, agent.player.transform.position.z);
        agent.RotateTo(lockY, agent.config.rotationSpeed);
    }

    private void CalculateNearlyCharge(Agent agent)
    {
        if (preparingChargeTimer.GetElapsedTime() >= nearlyCharge)
        {
            agent.config.isNearlyCharge = true;
        }
        else
        {
            agent.config.isNearlyCharge = false;
        }
    }

    private void PreparingChargeFinished()
    {
        doCharge = true;
    }

    private void ChargeFinished()
    {
        activatePhysics = true;
    }
    
    private void Charge(Agent agent)
    {
        Vector3 direction = agent.player.transform.position - agent.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        direction = direction.normalized;
        Vector3 forceToApply = direction * agent.config.chargeForce;
        
        agent.rigidbody.AddForce(forceToApply, ForceMode.VelocityChange);
        agent.transform.rotation = Quaternion.identity;
    }

    private void DesactivatePhysiscs(Agent agent)
    {
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;
    }
    
    private void ActivatePhysics(Agent agent)
    {
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
    }
}