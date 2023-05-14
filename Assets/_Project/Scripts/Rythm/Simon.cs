using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Simon : MonoBehaviour
{
    [SerializeField] private ThrowingSystem throwingSystem;
    [SerializeField] private SimonController simonController;
    [SerializeField] private DecalProjector graffiti;
    [SerializeField] private CustomSimonEvent simonEvent;

    private void Awake()
    {
        simonEvent.OnTrigger += simonController.PlaySimon;
    }
    

}