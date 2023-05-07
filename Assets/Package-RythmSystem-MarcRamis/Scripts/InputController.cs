using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{
    public static InputController instance;
    
    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool CheckIfCorrectButton(EControlType controlCorrect)
    {
        EControlType _controlPressed = PressedControl();

        if (_controlPressed == controlCorrect)
        {
            Debug.Log("true");
            return true;
        }
        
        return false;
    }
    
    private EControlType PressedControl()
    {
        if (Input.GetButtonDown("Cross"))
       {
            Debug.Log("cross");
            return EControlType.CROSS;
       }
       
       if (Input.GetButtonDown("Circle"))
       {
            Debug.Log("circle");
            return EControlType.CIRCLE;
       }

       if (Input.GetButtonDown("Square"))
       {
            Debug.Log("square");
            return EControlType.SQUARE;
       }

       if (Input.GetButtonDown("Triangle"))
       {
            Debug.Log("triangle");
            return EControlType.TRIANGLE;
       }

        return EControlType.NONE;
    }
}