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
        Invoke("playDebrisAnim", debrisAnimDelay);
        particles1.Play();
        particles2.Play();
        particles3.Play();
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
