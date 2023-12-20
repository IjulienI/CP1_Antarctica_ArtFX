using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image tutorialBackgroundImg;
    [SerializeField] private Sprite[] tutorialSprite;
    [SerializeField] private Image tutorialImage;

    private bool isFullShow = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFullShow)
        {
            tutorialImage.sprite = tutorialSprite[0];
            tutorialBackgroundImg.GetComponent<Animator>().SetBool("FadeIn", true);
            isFullShow = true;
            Invoke(nameof(NoMoreTuto), 10f);
        }       
    }

    private void NoMoreTuto()
    {  
        if (tutorialBackgroundImg != null)
        {
            tutorialBackgroundImg.GetComponent<Animator>().SetBool("FadeIn", false);
        }
        Invoke(nameof(SetIsFullShowed), 1.3f);
    }

    private void SetIsFullShowed()
    {
        isFullShow = false;
    }
}
