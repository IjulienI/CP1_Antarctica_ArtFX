using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionIconScript : MonoBehaviour
{
    [SerializeField] private GameObject keybindGo;
    [SerializeField] private Sprite keyboardKeybind;
    [SerializeField] private Sprite gamepadKeybind;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (DeviceDetection.instance.GetIsKeyboard())
            {
                keybindGo.GetComponent<SpriteRenderer>().sprite = keyboardKeybind;

            }
            else if (DeviceDetection.instance.GetIsGamepad())
            {
                keybindGo.GetComponent<SpriteRenderer>().sprite = gamepadKeybind;
            }
            keybindGo.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            keybindGo.SetActive(false);
        }
    }
}
