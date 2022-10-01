using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDirection : MonoBehaviour
{
    public Transform cameraView;
    public float coinDistance;

    void FixedUpdate()
    {
        transform.localPosition = cameraView.forward * coinDistance;
    }
}
