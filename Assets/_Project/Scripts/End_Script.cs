using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class End_Script : MonoBehaviour
{
    [SerializeField] private TextMeshPro endText;
    [SerializeField] private DecalProjector graffity = null;

    [SerializeField] private int numberOffActivators = 2;
    [SerializeField] private float graffitySpeed = 0.15f;
    private int currentActivators = 0;
    private bool startPainting = false;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.transform.tag == "Player")
    //    {
    //        endText.DOFade(1, 1.0f);
    //    }
    //}

    private void Update()
    {
        if(startPainting)
            graffity.fadeFactor += Time.deltaTime * graffitySpeed;
    }

    public void RevealGraffity()
    {
        currentActivators++;

        if (currentActivators >= numberOffActivators)
        {
            startPainting = true;
        }
    }



}
