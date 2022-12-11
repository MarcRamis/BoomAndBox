using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
    public GameObject endPosition;
    private Vector3 startPosition;
    public float time = 3f;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        float x = elapsedTime / time;

        transform.position = Vector3.Lerp(startPosition,endPosition.transform.position, x);
    }
}
