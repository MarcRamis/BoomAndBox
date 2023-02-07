using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayer : MonoBehaviour
{
    private GameObject player;
    
    public bool attachToOrientation = false;
    public bool attachToCompanion = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            transform.parent = player.transform.GetChild(1).GetChild(1);
        }
    }
}
