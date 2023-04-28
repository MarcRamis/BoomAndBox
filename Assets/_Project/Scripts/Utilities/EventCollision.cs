using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollision : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string[] tags = null;
    [Header("Name")]
    [SerializeField] private string[] names = null;
    [Header("Layer")]
    [SerializeField] private string[] layers = null;

    [Header("Unity Events")]
    [SerializeField] UnityEvent CollisionEnter_Event;
    [SerializeField] UnityEvent CollisionExit_Event;

    private bool enter = false;

    private void Awake()
    {
        if (CollisionEnter_Event == null)
            CollisionEnter_Event = new UnityEvent();

        if (CollisionExit_Event == null)
            CollisionExit_Event = new UnityEvent();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SearchForObject(collision))
        {
            CollisionEnter_Event?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (SearchForObject(collision))
        {
            CollisionExit_Event?.Invoke();
        }
    }


    private bool SearchForObject(Collision other)
    {
        foreach (var tag in tags)
        {
            if (tag == other.gameObject.tag)
            {
                return true;
            }
        }


        foreach (var name in names)
        {
            if (name == other.gameObject.name)
            {
                return true;
            }
        }


        foreach (var layer in layers)
        {
            if (LayerMask.NameToLayer(layer) == other.gameObject.layer)
            {
                return true;
            }
        }

        return false;
    }
}
