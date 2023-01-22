using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isMovable = false;
    [SerializeField] private bool isWaiting = false;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private AnimationCurve moveCurveSmooth;
    [SerializeField] private float velocity;
    [SerializeField] private float offSetToChangeDirection;
    [SerializeField] private float timeToWait;
    private float elapsedTime;
    private bool reachDestination = false;
    private bool wait = false;

    private void FixedUpdate()
    {
        if (isMovable && !wait)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        elapsedTime += Time.fixedDeltaTime * velocity;

        if (reachDestination)
        {
            transform.position = Vector3.MoveTowards(endPosition.position, startPosition.position, elapsedTime);
            if(Vector3.Distance(transform.position, startPosition.position) <= offSetToChangeDirection)
            {
                reachDestination = false;
                elapsedTime = 0.0f;
                if(isWaiting)
                {
                    wait = true;
                    Invoke(nameof(WaitTimer), timeToWait);
                }
            }

        }
        else
        {
            transform.position = Vector3.MoveTowards(startPosition.position, endPosition.position, elapsedTime);
            if (Vector3.Distance(transform.position, endPosition.position) <= offSetToChangeDirection)
            {
                reachDestination = true;
                elapsedTime = 0.0f;
                if (isWaiting)
                {
                    wait = true;
                    Invoke(nameof(WaitTimer), timeToWait);
                }
            }
        }



    }

    public void ChangeMoveableState()
    {
        isMovable = !isMovable;
    }

    private void WaitTimer()
    {
        wait = false;
    }
}
