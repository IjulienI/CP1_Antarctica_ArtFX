using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ElevatorTilemapChange : MonoBehaviour
{
    [SerializeField] private GameObject tilemapBefore;
    [SerializeField] private GameObject tilemapAfter;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] AudioSource AudioSource;

    public void TilemapChange()
    {
        particles.Play();
        StartCoroutine(GameObject.Find("Virtual Camera").GetComponent<CameraShake>().Shake(0.8f, 16));
        Invoke("DelayChange", 0.3f);
    }

    private void DelayChange()
    {
        tilemapBefore.SetActive(false);
        tilemapAfter.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void MakeSound()
    {
        AudioSource.Play();
    }
    public void Rumble()
    {
        RumbleGamepad.instance.MakeGampadRumble(0.4f, 0.4f, 0.2f);
    }
}
