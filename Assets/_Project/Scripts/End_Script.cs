using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class End_Script : MonoBehaviour
{
    [SerializeField] private TextMeshPro endText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            endText.DOFade(1, 1.0f);
        }
    }
}
