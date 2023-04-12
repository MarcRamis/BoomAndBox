using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [HideInInspector] private Player player;
    [HideInInspector] private RythmSystem rythmSystem;

    private bool rythmMoment;

    private void Awake()
    {
        player = GetComponent<Player>();
        rythmSystem = GetComponent<RythmSystem>();

        player.myInputs.OnAttackPerformed += DoAttack;
    }
    
    private void FixedUpdate()
    {
        rythmMoment = rythmSystem.IsRythmMoment();
      
    }

    private void DoAttack()
    {
        if(player.CanAttack())
        {
            if (rythmMoment)
            {
                Debug.Log("Attack on rythm");
            }
            else
            {
                Debug.Log("Attack normal");
            }
        }
    }
}
