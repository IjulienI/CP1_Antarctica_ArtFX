using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SASTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem SASparticlesR;
    [SerializeField] private ParticleSystem SASparticlesL;

    [SerializeField] private Collider2D DoorColR;
    [SerializeField] private Collider2D DoorColL;

    [SerializeField] private Light2D LightR;
    [SerializeField] private Light2D LightL;

    [SerializeField] private AudioSource sasAudioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SASparticlesR.Play();
        SASparticlesL.Play();
        sasAudioSource.Play();

        DoorColR.enabled = false;
        if(DoorColL != null)
        {
            DoorColL.enabled = false;
        }

        LightR.intensity = 1;
        LightL.intensity = 1;

        gameObject.GetComponent<Collider2D>().enabled = false;

        Invoke("EnableDoors", 4);
    }

    private void EnableDoors()
    {
        if(DoorColR != null)
        {
            DoorColR.enabled = true;
        }
        if (DoorColL != null)
        {
            DoorColL.enabled = true;
        }    
    }
}
