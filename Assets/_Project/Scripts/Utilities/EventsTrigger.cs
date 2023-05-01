using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsTrigger : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string[] tags = null;
    [Header("Name")]
    [SerializeField] private string[] names = null;
    [Header("Layer")]
    [SerializeField] private string[] layers = null;

    [Header("Unity Events")]
    [SerializeField] UnityEvent TriggerEnter_Event;
    [SerializeField] UnityEvent TriggerExit_Event;

    private bool enter = false;

    private void Awake()
    {
        if (TriggerEnter_Event == null)
            TriggerEnter_Event = new UnityEvent();

        if (TriggerExit_Event == null)
            TriggerExit_Event = new UnityEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(SearchForObject(other))
        {
            TriggerEnter_Event?.Invoke();
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (SearchForObject(other))
        {
            TriggerExit_Event?.Invoke();
        }
    }

    private bool SearchForObject(Collider other)
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
