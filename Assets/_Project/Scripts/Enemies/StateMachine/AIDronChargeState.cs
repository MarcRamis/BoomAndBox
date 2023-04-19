using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDronChargeState : IAIState
{
    // Internal variables
    private MTimer chargeTimer;
    private MTimer preparingChargeTimer;
    private float timerLoop;
    private GameObject chargePrefab;

    private bool activatePhysics = false;
    private bool doCharge = false;
    
    private const float chargeTime = 0.5f;
    private const float preparingChargeTime = 2f;
    private const float preparingChargeTimeAfterFirstRound = 5f;
    
    public void Enter(Agent agent)
    {
        chargeTimer = new MTimer();
        chargeTimer.OnTimerEnd += ChargeFinished;
        
        preparingChargeTimer = new MTimer();
        preparingChargeTimer.SetTimeLimit(preparingChargeTime);
        preparingChargeTimer.OnTimerEnd += PreparingChargeFinished;
        preparingChargeTimer.StartTimer();

        //agent.feedbackController.PlayPreparingCharge();

        timerLoop = agent.config.timePreparingToCharge;
        
        DesactivatePhysiscs(agent);
    }
    
    public void Exit(Agent agent)
    {
        agent.feedbackController.StopPreparingCharge();
        agent.feedbackController.StopCharge();
 
        ActivatePhysics(agent);
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

        if (doCharge)
        {
            Debug.Log("entr carga");
            doCharge = false;
            DesactivatePhysiscs(agent);
            Charge(agent);

            chargeTimer.SetTimeLimit(chargeTime);
            chargeTimer.StartTimer();
        }
        
        if (activatePhysics)
        {
            Debug.Log("entr prepar");
            activatePhysics = false;
            ActivatePhysics(agent);

            preparingChargeTimer.SetTimeLimit(preparingChargeTimeAfterFirstRound);
            preparingChargeTimer.StartTimer();
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
        direction = direction.normalized;
        Vector3 forceToApply = direction * agent.config.chargeForce;
        
        agent.rigidbody.AddForce(forceToApply, ForceMode.VelocityChange);
        agent.transform.rotation = Quaternion.identity;
    }

    private void DesactivatePhysiscs(Agent agent)
    {
        agent.config.isCharging = true;
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;
        agent.transform.rotation = Quaternion.identity;
    }
    
    private void ActivatePhysics(Agent agent)
    {
        agent.config.isCharging = false;
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
        agent.navMesh.angularSpeed = 10000;
        agent.transform.rotation = Quaternion.identity;
    }
}