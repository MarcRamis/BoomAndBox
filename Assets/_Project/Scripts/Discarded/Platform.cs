using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //[Header("References")]
    //private Collider m_Collider;
    //[SerializeField] private Material solidMaterial;
    //[SerializeField] private Material tangibleMaterial;
    
    [SerializeField] public enum EPlatformType { DEFAULT, TANGNONTANG}
    [Header("Settings")]
    [SerializeField] private EPlatformType platformType;
    [SerializeField] private bool isMovable = false;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float moveTime;
    [SerializeField] private AnimationCurve moveCurveSmooth;
    private Vector3 currentPos;
    private float elapsedTime;
    private bool reachDestination;

    // Switcher
    //[SerializeField] public enum EPlatformState { SOLID, NONTANGIBLE,}
    //[SerializeField] private EPlatformState platformState;

    // Awake
    private void Awake()
    {
        //m_Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        // switcher
        //HandleTangibleState();
    }
    
    private void FixedUpdate()
    {
        if (isMovable)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        elapsedTime += Time.fixedDeltaTime;
        float percentageComplete = elapsedTime / moveTime;

        if (reachDestination)
        {
            currentPos = Vector3.Lerp(endPosition.position, startPosition.position, moveCurveSmooth.Evaluate(percentageComplete));
        }
        else
        {
            currentPos = Vector3.Lerp(startPosition.position, endPosition.position, moveCurveSmooth.Evaluate(percentageComplete));
        }
        GetComponent<Rigidbody>().MovePosition(currentPos);

        if (percentageComplete >= 1.0f)
        {
            reachDestination = !reachDestination;
            percentageComplete = 0.0f;
            elapsedTime = 0.0f;
        }

    }

    // Functions
    //public void HandleTangibleState()
    //{
    //    switch (platformState)
    //    {
    //        case EPlatformState.SOLID:
    //
    //            m_Collider.enabled = true;
    //            GetComponent<Renderer>().material= solidMaterial; 
    //            break;
    //        
    //        case EPlatformState.NONTANGIBLE:
    //
    //            m_Collider.enabled = false;
    //            GetComponent<Renderer>().material = tangibleMaterial;
    //            break;
    //    }
    //}

    //public void ChangeState(EPlatformState _state)
    //{
    //    platformState = _state;
    //}
}