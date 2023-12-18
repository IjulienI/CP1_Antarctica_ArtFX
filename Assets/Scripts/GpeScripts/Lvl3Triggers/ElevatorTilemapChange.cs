using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTilemapChange : MonoBehaviour
{
    [SerializeField] private GameObject tilemapBefore;
    [SerializeField] private GameObject tilemapAfter;

    public void TilemapChange()
    {
        StartCoroutine(GameObject.Find("Virtual Camera").GetComponent<CameraShake>().Shake(0.8f, 16));

        tilemapBefore.SetActive(false);
        tilemapAfter.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
