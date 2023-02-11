using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTo : MonoBehaviour
{
    private GameObject player;
    private GameObject companion;
    
    public bool attachToOrientation = false;
    public bool attachToCompanion = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        companion = GameObject.FindGameObjectWithTag("Companion");
        if (!attachToOrientation) transform.parent = player.transform;
    }
    private void Update()
    {
        if (attachToOrientation)
        {
            transform.parent = player.transform.GetChild(1);
        }
        if (attachToCompanion)
        {
            transform.parent = companion.transform;
        }
    }
}
