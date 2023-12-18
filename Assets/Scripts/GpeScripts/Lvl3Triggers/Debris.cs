using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private GameObject debrisObject;
    [SerializeField] private float delay;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        debrisObject.GetComponent<Animator>().enabled = true;
        Invoke("Shake", delay);
    }

    private void Shake()
    {
        StartCoroutine(GameObject.Find("Virtual Camera").GetComponent<CameraShake>().Shake(shakeDuration, shakeAmount));
    }
}
