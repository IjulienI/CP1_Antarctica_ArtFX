using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public bool targetIn = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Alien"))
        {
            collision.GetComponent<AiStateMachine>().zone = gameObject;
        }
        if(collision.tag == "Target")
        {
            targetIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Target")
        {
            targetIn = false;
        }
    }
}
