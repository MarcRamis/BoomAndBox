using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFeedbackController : FeedbackController
{
    
    [SerializeField] private AudioClip hitSoundSmall;
    [SerializeField] private AudioClip hitSoundBig;
    [SerializeField] private AudioClip hitSoundBig2;
    
    [Space]
    [SerializeField] private TrailRenderer trail1;
    [SerializeField] private TrailRenderer trail2;
    [SerializeField] private TrailRenderer trail3;
    [SerializeField] private TrailRenderer trail4;

    private bool hitOnce = true;
    private float hitCd;

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

    /////////// HIT IMPACT
    public void PlayHitImpact(int counter)
    {
        if (hitOnce)
        {
            hitOnce = false;
            Invoke(nameof(ResetHitOnce), 0.5f);

            switch ((counter))
            {
                case 0:

                    PlaySoundEffect(hitSoundSmall);

                    break;
                case 1:

                    PlaySoundEffect(hitSoundSmall);

                    break;
                case 2:
                    
                    int randomCount = Random.Range(1, 2);
                    switch ((randomCount))
                    {
                        case 1:

                            PlaySoundEffect(hitSoundBig);
                            break;

                        case 2:
                            PlaySoundEffect(hitSoundBig2);
                            break;
                    };
                    break;
            };
        }
    }

    private void ResetHitOnce()
    {
        hitOnce = true;
    }
}
