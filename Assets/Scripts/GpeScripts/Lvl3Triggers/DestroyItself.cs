using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("test");
        gameObject.SetActive(false);
    }
}
