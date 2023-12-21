using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject invisibleWallL;
    [SerializeField] private GameObject invisibleWallR;
    [SerializeField] private GameObject invisibleWallT;
    [SerializeField] private Animator elevatorAnimator;
    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private AudioSource elevatorAudio;

    [SerializeField] private Animator LightLAnimator;
    [SerializeField] private Animator LightRAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        elevatorAudio.Play();
        LightLAnimator.enabled = true;
        LightRAnimator.enabled = true;

        invisibleWallL.SetActive(true);
        invisibleWallR.SetActive(true);
        invisibleWallT.SetActive(true);
        Invoke("ElevatorStart", 1);
        Invoke("ZoomOutCam", 8);
    }

    private void ZoomOutCam()
    {
        cameraAnimator.enabled = true;
    }

    private void ElevatorStart()
    {
        elevatorAnimator.enabled = true;
    }
}
