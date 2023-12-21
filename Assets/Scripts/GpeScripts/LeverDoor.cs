using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class LeverDoor : MonoBehaviour
{
    [Header("Input Manager (don't touch)")]
    [SerializeField] InputActionReference interact;
    bool onLeverZone = false;
    public bool doorIsOpen = false;
    public int index;

    [SerializeField] Light2D LEDlight;
    [SerializeField] Animator animator;
    [SerializeField] Sprite leverOff;
    [SerializeField] Sprite leverOn;
    [SerializeField] SpriteRenderer leverRenderer;

    private void Start()
    {
        Invoke(nameof(Load), 0.5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onLeverZone =true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onLeverZone = false;
        }
    }

    private void OnEnable()
    {
        interact.action.started += Interaction;
    }

    private void Interaction(InputAction.CallbackContext obj)
    {
        if (onLeverZone) 
        {
            doorIsOpen = !doorIsOpen;

            Sprite nextSprite = leverRenderer.sprite == leverOff ? leverOn : leverOff;
            leverRenderer.sprite = nextSprite;

            LEDlight.color = Color.green;

            if (doorIsOpen == true)
            {
                animator.SetBool("OpenDoor", true);
            }
            else if (doorIsOpen == false)
            {
                animator.SetBool("OpenDoor", false);
            }
        }

    }

    private void Load()
    {
        if (doorIsOpen == true)
        {
            animator.SetBool("OpenDoor", true);
        }
    }
}
