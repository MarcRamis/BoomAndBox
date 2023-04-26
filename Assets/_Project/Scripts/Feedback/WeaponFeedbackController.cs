using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFeedbackController : FeedbackController
{
    [SerializeField] private TrailRenderer trail1;
    [SerializeField] private TrailRenderer trail2;
    [SerializeField] private TrailRenderer trail3;
    [SerializeField] private TrailRenderer trail4;


    /////////// MAKE DAMAGE
    public void PlayAttack()
    {
        //trail1.emitting = true;
        trail2.emitting = true;
        //trail4.emitting = true;
        //trail3.emitting = true;
    }
    
    public void StopAttack()
    {
        //trail1.emitting = false;
        trail2.emitting = false;
        //trail3.emitting = false;
        //trail4.emitting = false;
    }
}
