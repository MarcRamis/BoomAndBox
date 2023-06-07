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

    // Enter the state: This method is called when entering the AIDronChargeState.
    // It initializes the chargeTimer and preparingChargeTimer and sets their respective event callbacks.
    // It also sets the nearlyCharge value based on the preparingChargeTime and nearlyChargePercentage.
    // It deactivates physics, plays the preparing charge animation, and starts the preparingChargeTimer.
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
        agent.feedbackController.PlayPreparingCharge();
    }

    // Exit the state: This method is called when exiting the AIDronChargeState.
    // It stops the preparing charge and charge animations.
    // It activates physics, clears the player's beingTargettedBy reference, and resets the agent's preparingChargeTime.
    public void Exit(Agent agent)
    {
        agent.feedbackController.StopPreparingCharge();
        agent.feedbackController.StopCharge();

        ActivatePhysics(agent);

        agent.playerScript.beingTargettedBy = null;
    }

    // GetId: Returns the unique identifier for this state, which is EAIState.CHARGE.
    public EAIState GetId()
    {
        return EAIState.CHARGE;
    }

    // Update the state: This method is called during the update loop.
    // It calls the OnUpdate method of the agent's manager, passing the agent as a parameter.
    // It updates the chargeTimer and preparingChargeTimer.
    // It calculates the nearlyCharge value based on the elapsed time of the preparingChargeTimer.
    // If doCharge is true, it stops the preparing charge animation, plays the charge animation,
    // deactivates physics, and calls the Charge method.
    // If activatePhysics is true, it stops the charge animation, plays the preparing charge animation,
    // activates physics, increases the preparingChargeTime, and starts the preparingChargeTimer.
    // Finally, it calls the LookAtPlayer method to make the agent face the player.
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);
        chargeTimer.Update(Time.fixedDeltaTime);
        preparingChargeTimer.Update(Time.fixedDeltaTime);

        CalculateNearlyCharge(agent);

        if (doCharge)
        {
            agent.feedbackController.StopPreparingCharge();
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
            agent.feedbackController.PlayPreparingCharge();
            activatePhysics = false;
            ActivatePhysics(agent);

            preparingChargeTime += sumPreparingChargeTime;
            preparingChargeTimer.SetTimeLimit(preparingChargeTime);
            preparingChargeTimer.StartTimer();
        }

        LookAtPlayer(agent);
    }

    // LookAtPlayer: Makes the agent face the player by rotating towards their position.
    private void LookAtPlayer(Agent agent)
    {
        Vector3 lockY = new Vector3(agent.player.transform.position.x, agent.player.transform.position.y, agent.player.transform.position.z);
        agent.RotateTo(lockY, agent.config.rotationSpeed);
    }

    // CalculateNearlyCharge: Calculates whether the agent is nearly charged based on the elapsed time of the preparingChargeTimer.
    // If the elapsed time is greater than or equal to nearlyCharge, it sets the isNearlyCharge flag in the agent's config to true.
    // Otherwise, it sets the flag to false.
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

    // PreparingChargeFinished: Event callback method for when the preparingChargeTimer ends.
    // It sets the doCharge flag to true, indicating that the agent should perform a charge.
    private void PreparingChargeFinished()
    {
        doCharge = true;
    }

    // ChargeFinished: Event callback method for when the chargeTimer ends.
    // It sets the activatePhysics flag to true, indicating that physics should be activated.
    private void ChargeFinished()
    {
        activatePhysics = true;
    }

    // Charge: Applies a charge force to the agent in the direction of the player's position.
    // It calculates the direction vector from the agent to the player, ignoring the y-axis.
    // It normalizes the direction vector and multiplies it by the chargeForce value from the agent's config.
    // It then applies the force to the agent's rigidbody and resets its rotation.
    private void Charge(Agent agent)
    {
        Vector3 direction = agent.player.transform.position - agent.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        direction = direction.normalized;
        Vector3 forceToApply = direction * agent.config.chargeForce;

        agent.rigidbody.AddForce(forceToApply, ForceMode.VelocityChange);
        agent.transform.rotation = Quaternion.identity;
    }

    // DesactivatePhysiscs: Deactivates physics for the agent by setting isKinematic to false and disabling the NavMesh.
    private void DesactivatePhysiscs(Agent agent)
    {
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;
    }

    // ActivatePhysics: Activates physics for the agent by setting isKinematic to true and enabling the NavMesh.
    private void ActivatePhysics(Agent agent)
    {
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
    }
}
