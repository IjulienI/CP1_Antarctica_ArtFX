using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicDoor : MonoBehaviour
{
    [SerializeField] Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("OpenDoor", true);
            //animation ouverture porte collider
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("OpenDoor", false);
            //animation fermeture porte collider
        }
    }

}
