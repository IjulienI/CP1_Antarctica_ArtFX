using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectedSound : MonoBehaviour, ISelectHandler
{
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioClip selectedButtonSFX;

    public void OnSelect(BaseEventData eventData)
    {
        if (audioSourceSFX != null && selectedButtonSFX != null)
        {
            audioSourceSFX.clip = selectedButtonSFX;
            audioSourceSFX.Play();
        }
    }
}
