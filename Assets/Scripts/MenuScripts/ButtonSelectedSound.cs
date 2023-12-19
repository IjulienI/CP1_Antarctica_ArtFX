using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectedSound : MonoBehaviour, ISelectHandler
{
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioClip selectedButtonSFX;
    public void OnSelect(BaseEventData eventData)
    {
        audioSourceSFX.clip = selectedButtonSFX;
        audioSourceSFX.Play();
    }
}
