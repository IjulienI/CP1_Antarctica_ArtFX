using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightsSequence : MonoBehaviour
{
    [SerializeField] private GameObject spotlights;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spotlights.SetActive(true);
    }
}
