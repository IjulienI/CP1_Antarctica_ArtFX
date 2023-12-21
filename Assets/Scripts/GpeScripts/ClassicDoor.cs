using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicDoor : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource soundDoor;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Alien")
        {
            animator.SetBool("OpenDoor", true);
            soundDoor.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Alien")
        {
            animator.SetBool("OpenDoor", false);
            soundDoor.Play();
        }
    }
}
