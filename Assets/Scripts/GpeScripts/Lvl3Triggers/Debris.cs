using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private GameObject debrisObject;
    [SerializeField] private ParticleSystem particles1;
    [SerializeField] private ParticleSystem particles2;
    [SerializeField] private ParticleSystem particles3;
    [SerializeField] private float debrisAnimDelay;
    [SerializeField] private float shakeDelay;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(debrisObject != null)
        {
            Invoke("playDebrisAnim", debrisAnimDelay);
        }
        if (particles1 != null)
        {
            particles1.Play();
        }
        if (particles2 != null)
        {
            particles2.Play();
        }
        if (particles3 != null)
        {
            particles3.Play();
        }
        Invoke("Shake", shakeDelay);
    }

    private void Shake()
    {
        StartCoroutine(GameObject.Find("Virtual Camera").GetComponent<CameraShake>().Shake(shakeDuration, shakeAmount));
    }

    private void playDebrisAnim()
    {
        debrisObject.GetComponent<Animator>().enabled = true;
    }
}
