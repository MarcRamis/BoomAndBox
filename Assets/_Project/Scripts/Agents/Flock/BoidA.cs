using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidA : Enemy
{
    public override void OnDeath()
    {
        feedbackController.PlayDeath();
        base.OnDeath();
    }
}
