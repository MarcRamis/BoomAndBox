using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidA : MonoBehaviour, IDamageable
{
    public BoidFeedbackController feedbackController;
    public SimpleComputePersistent simpleComputePersistent;

    public int Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int health;

    private void OnDeath()
    {
        feedbackController.PlayDeath();
        Invoke(nameof(Destroying), 0.5f);
    }

    public void Damage(int damageAmount)
    {
        OnDeath();
    }

    public void Knockback(float force)
    {
    }

    private void Destroying()
    {
        simpleComputePersistent.objects.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
