using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractBomb : MonoBehaviour
{
    private bool oui = false;
    [SerializeField]  GameObject animator;
    void Update()
    {
        if (animator.GetComponent<PlantTheSpike>().IsSpikePlanted())
        {
            SceneManager.LoadScene("Level 3");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        oui = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        oui = false;
    }
}
