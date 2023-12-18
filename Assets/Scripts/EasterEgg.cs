using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EasterEgg : MonoBehaviour
{
    [Header("Don't Touch")]

    public InputActionReference input1;
    public InputActionReference input2;
    public InputActionReference input3;

    bool step1 = false;
    bool step2 = false;
    bool step3 = false;

    [SerializeField] GameObject Yobb;

    private void OnEnable()
    {
        input1.action.started += FirstInput;
        input2.action.started += SecondInput;
        input3.action.started += ThirdInput;
    }


    private void FirstInput(InputAction.CallbackContext obj)
    {
        step1 = true;
        Invoke("RestartEegg", 5f);
        step2 = false;
        step3 = false;
    }

    private void SecondInput(InputAction.CallbackContext obj)
    {
        if (step1 == true)
        {
            step2 = true;
            step3 = false;
        }
    }

    private void ThirdInput(InputAction.CallbackContext obj)
    {
        if (step2 == true && step3 == false)
        {
            step3 = true;
        }
        else if (step3 == true)
        {
            Yobb.SetActive(!Yobb.active);
            step1 = false;
            step2 = false;
            step3 = false;
        }
    }

    private void RestartEegg()
    {
        step1 = false;
        step2 = false;
        step3 = false;
    }
}
