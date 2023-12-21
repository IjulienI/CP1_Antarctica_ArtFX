using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MucusGO : MonoBehaviour
{
    [SerializeField] GameObject mucusGO;
    void Start()
    {
        mucusGO.SetActive(false);
        Invoke("ActiveScreen", 5.1f); 
    }

    private void ActiveScreen()
    {
        mucusGO.SetActive(true);
    }
}
