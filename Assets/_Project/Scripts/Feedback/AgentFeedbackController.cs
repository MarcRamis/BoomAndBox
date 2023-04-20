using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class AgentFeedbackController : FeedbackController
{
    /////////// TAKE DAMAGE
    public virtual void PlayTakeDamage()
    {
    }

    /////////// DIE
    public virtual void PlayDeath()
    {
    }

    /////////// PREPARING CHARGE
    public virtual void PlayPreparingCharge()
    {
    }

    public virtual void StopPreparingCharge()
    {
    }

    /////////// CHARGE
    public virtual void PlayCharge()
    {
    }

    public virtual void StopCharge()
    {
    }

    /////////// IMPULSE
    public virtual void PlayImpulse()
    {
    }
    
    /////////// WALK
    public virtual void PlayWalk()
    {
    }
    /////////// RUN
    public virtual void PlayRun()
    {
    }
}
