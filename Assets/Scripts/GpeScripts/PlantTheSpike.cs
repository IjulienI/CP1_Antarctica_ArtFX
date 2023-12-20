using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantTheSpike : MonoBehaviour
{
    private bool isInZone;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private GameObject spikeGo;
    private bool isInteractPressed;
    private bool bombPlanted;

    private void OnEnable()
    {
        interact.action.started += OnInteractStarted;
        interact.action.canceled += OnInteractCanceled;
    }

    private void OnDisable()
    {
        interact.action.started -= OnInteractStarted;
        interact.action.canceled -= OnInteractCanceled;
    }
    private void Update()
    {
        int isActivated = PlayerPrefs.GetInt("Vibration activation", 1);
        if (isInteractPressed && !bombPlanted && isActivated == 1)
        {
            if(Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0.1f, 0.2f);
            }
            Debug.Log("Interact is being held!");
        }
        else if(Gamepad.current != null && isInZone)
        {

            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        if (isInZone && !bombPlanted)
        {
            isInteractPressed = true;
            Invoke("CheckHoldDuration", 4f);
        }
    }
    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        print("testasfasfasf");
        isInteractPressed = false;
        CancelInvoke("CheckHoldDuration");
    }
    void CheckHoldDuration()
    {
        bombPlanted = true;
        spikeGo.GetComponent<Animator>().SetTrigger("Play");
        Debug.Log("Interact held for 4 seconds!");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("player");
            isInZone = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isInZone = false;
    }
}
