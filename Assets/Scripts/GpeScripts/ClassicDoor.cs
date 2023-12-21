using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicDoor : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource soundDoor;
    [SerializeField] bool isNeedingBomb;
    bool unlockedDoor;
    [SerializeField] GameObject spike;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Alien")
        {
            if (isNeedingBomb && spike.GetComponent<PlantTheSpike>().IsSpikePlanted())
            {
                animator.SetBool("OpenDoor", true);
                unlockedDoor = true;
                soundDoor.Play();
            }
            else if (!isNeedingBomb)
            {
                animator.SetBool("OpenDoor", true);
                unlockedDoor = true;
                soundDoor.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Alien") && unlockedDoor)
        {
            animator.SetBool("OpenDoor", false);
            soundDoor.Play();
        }
    }
}
