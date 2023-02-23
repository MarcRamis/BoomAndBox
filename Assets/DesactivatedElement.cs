using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivatedElement : MonoBehaviour
{
    [SerializeField] private Transform positionToBe;

    void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponent<MMFeedbackPosition>().Initialization(child);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = positionToBe.position;
    }

    private void OnEnable()
    {
        transform.position = positionToBe.position;
    }

}
