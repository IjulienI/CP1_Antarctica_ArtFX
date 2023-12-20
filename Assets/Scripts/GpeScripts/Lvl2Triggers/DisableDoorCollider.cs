using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDoorCollider : MonoBehaviour
{
    public void DisableCollier()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
