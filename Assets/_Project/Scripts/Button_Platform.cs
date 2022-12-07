using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Platform : MonoBehaviour
{
    [Header("Platform list")]
    [SerializeField] private GameObject[] platformsToChange;

    [Header("Settings")]
    [SerializeField] private PlatformAction actionToDo;

    //Private variables
    private enum PlatformAction
    {
        Move
    };
    private bool timer = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Trigger");
        if (collision.gameObject.tag == "Companion" && !timer)
        {
            Debug.Log("Entro");
            switch (actionToDo)
            {
                case PlatformAction.Move:
                    foreach (var platform in platformsToChange)
                    {
                        platform.GetComponentInChildren<MoveablePlatform>().ChangeMoveableState();
                        Debug.Log("ForEach");
                    }
                    break;
            }
            //timer = true;
            //Invoke(nameof(Timer), 0.25f);

        }
    }

    private void Timer()
    {
        timer = false;
    }

}
