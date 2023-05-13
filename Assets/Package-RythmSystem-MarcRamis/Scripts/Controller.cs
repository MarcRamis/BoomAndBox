using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject boxDamage;
    private bool isRythmMoment;
    private bool canRythm = false;
    
    [Header("Settings")]
    public float rythmCd = 0.5f;
    public float feedbackDuration = 0.2f;
    public float finalComboDuration = 2f;
    
    [Header("Feedback")]
    [SerializeField] private TrailRenderer trailRenderer_RythmTime;
    [SerializeField] private TrailRenderer trailRender_FinalCombo;
    [SerializeField] private Image imgTest;
    [SerializeField] private GameObject imgCheckTest;

    private float attackCd = 0.5f;
    private bool attackIsReady = true;

    private Combo combo = new Combo();
    
    private void Awake()
    {
        trailRenderer_RythmTime.emitting = false;
        trailRender_FinalCombo.emitting = false;

        combo.SetMaxCombo(3);
        RythmController.instance.beat.OnBeat += Rythm;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (attackIsReady)
            {
                attackIsReady = false;
                Invoke(nameof(ResetAttack), attackCd);
                
                if (canRythm)
                {
                    Debug.Log("Rythm Time!");
                    PlayCheckRythmMomentFeedback();                    
                    combo.SumCombo();
                }
                else
                {
                    combo.ComboFailed();
                }
                
                if (combo.ComboAccomplished())
                {
                    animator.SetTrigger("FinalCombo");
                    trailRender_FinalCombo.emitting = true;
                    Invoke(nameof(ResetEffect2), feedbackDuration);
                }
                else
                {
                    animator.SetTrigger("Punch");
                }
            }
        }
    }

    private void Rythm()
    {
        canRythm = true;
        PlayRythmMomentFeedback();
        Invoke(nameof(ResetRythm), rythmCd);
    }

    private void PlayRythmMomentFeedback()
    {
        imgTest.color = Color.yellow;
    }

    private void ResetAttack()
    {
        attackIsReady = true;
    }

    private void ResetRythm()
    {
        canRythm = false;
        StopRythmMomentFeedback();
    }
    
    private void StopRythmMomentFeedback()
    {
        imgTest.color = Color.white;
    }

    private void PlayCheckRythmMomentFeedback()
    {
        trailRenderer_RythmTime.emitting = true;
        imgCheckTest.SetActive(true);
        Invoke(nameof(ResetEffect1), feedbackDuration);
    }

    private void StopCheckRythmMomentFeedback()
    {
        trailRenderer_RythmTime.emitting = false;
        imgCheckTest.SetActive(false);
    }

    private void ResetEffect1()
    {     
        StopCheckRythmMomentFeedback();
    }

    private void ResetEffect2()
    {
        trailRender_FinalCombo.emitting = false;
    }
}
