using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SASDoor : MonoBehaviour
{
    [SerializeField] private Animator SASDoor1Anim;
    [SerializeField] private Animator SASDoor2Anim;
    [SerializeField] private ParticleSystem SASParticlesR;
    [SerializeField] private ParticleSystem SASParticlesL;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SASDoor1Anim.enabled = true;
        Invoke("CloseFirstDoor", 4);
        Invoke("Pchit", 5);
        Invoke("OpenSecondDoor", 7);
    }

    private void CloseFirstDoor()
    {
        SASDoor1Anim.SetTrigger("close");
    }

    private void Pchit()
    {
        SASParticlesR.Play();
        SASParticlesL.Play();
    }

    private void OpenSecondDoor()
    {
        SASDoor2Anim.enabled = true;
    }
}
