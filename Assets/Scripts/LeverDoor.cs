using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverDoor : MonoBehaviour
{
    [Header("Input Manager (don't touch)")]
    [SerializeField] InputActionReference interact;
    bool onLeverZone = false;
    bool doorIsOpen = false;

    [SerializeField] Animator animator;

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
        doorIsOpen = !doorIsOpen;

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
