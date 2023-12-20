using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ElevatorTilemapChange : MonoBehaviour
{
    [SerializeField] private GameObject tilemapBefore;
    [SerializeField] private GameObject tilemapAfter;
    [SerializeField] private ParticleSystem particles;

    public void TilemapChange()
    {
        particles.Play();
        StartCoroutine(GameObject.Find("Virtual Camera").GetComponent<CameraShake>().Shake(0.8f, 16));
        Invoke("DelayChange", 0.5f);
    }

    private void DelayChange()
    {
        tilemapBefore.SetActive(false);
        tilemapAfter.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
