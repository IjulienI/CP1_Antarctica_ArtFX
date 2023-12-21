using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlantTheSpike : MonoBehaviour
{
    private bool isInZone;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private GameObject spikeGo;
    [SerializeField] private GameObject progressBar;
    private bool isInteractPressed;
    private bool bombPlanted;
    [SerializeField] private Light2D lightSpike;
    [SerializeField] private GameObject keybindGo;
    [SerializeField] private Sprite keyboardKeybind;
    [SerializeField] private Sprite gamepadKeybind;
    [SerializeField] private AudioSource spikeAudioSource;
    bool isSelectButtonShowed;
    bool justExitTrigger;


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
                Gamepad.current.SetMotorSpeeds(0.05f, 0.1f);
            }
        }
        else if((Gamepad.current != null && isInZone)||justExitTrigger)
        {

            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        if (isInZone && !bombPlanted)
        {
            isInteractPressed = true;
            isSelectButtonShowed = false;
            keybindGo.SetActive(false);
            progressBar.SetActive(true);
            progressBar.GetComponent<Animator>().SetBool("Play", true);
            Invoke("CheckHoldDuration", 2.4f);
        }
    }
    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (isInZone && !bombPlanted)
        {
            isSelectButtonShowed = true;
            keybindGo.SetActive(true);
        }
        isInteractPressed = false;
        progressBar.SetActive(false);
        progressBar.GetComponent<Animator>().SetBool("Play", false);
        CancelInvoke("CheckHoldDuration");
    }
    void CheckHoldDuration()
    {
        isSelectButtonShowed = false;
        keybindGo.SetActive(false);
        bombPlanted = true;
        spikeAudioSource.Play();
        Invoke(nameof(LightOn), 0.3f);
        spikeGo.GetComponent<Animator>().SetTrigger("Play");
    }

    void LightOn()
    {
        lightSpike.intensity = Mathf.Lerp(0.5f, 1, 2.5f);
        lightSpike.pointLightOuterRadius = Mathf.Lerp(0.8f, 1.3f, 2.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInZone = true;
            if (DeviceDetection.instance.GetIsKeyboard())
            {
                keybindGo.GetComponent<SpriteRenderer>().sprite = keyboardKeybind;

            }
            else if (DeviceDetection.instance.GetIsGamepad())
            {
                keybindGo.GetComponent<SpriteRenderer>().sprite = gamepadKeybind;
            }
            if (!bombPlanted)
            {
                keybindGo.SetActive(true);
                isSelectButtonShowed = true;
            }
        }
   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        justExitTrigger = true;
        Invoke(nameof(SetJustTriggerExit), 0.1f);
        isInZone = false;
        isInteractPressed = false;
        CancelInvoke("CheckHoldDuration");
        if (collision.CompareTag("Player"))
        {
            keybindGo.SetActive(false);
            isSelectButtonShowed = false;
        }
    }
    private void SetJustTriggerExit()
    {
        justExitTrigger = false;
    }

    public bool IsSpikePlanted()
    {
        return bombPlanted;
    }
}
