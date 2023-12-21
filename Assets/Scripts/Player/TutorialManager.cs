using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image tutorialBackgroundImg;
    [SerializeField] private Sprite[] tutorialSprite;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private float showTimer;

    private bool isFullShow = false;

    [SerializeField] private TutoType tutoType;
    public enum TutoType
    {
        X,
        A,
        Triggers,
        Pause
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFullShow)
        {
            if (tutoType == TutoType.X)
            {
                tutorialImage.sprite = tutorialSprite[0];
            }
            else if (tutoType == TutoType.A)
            {
                tutorialImage.sprite = tutorialSprite[1];
            }
            else if (tutoType == TutoType.Triggers)
            {
                tutorialImage.sprite = tutorialSprite[2];
            }
            else if (tutoType == TutoType.Pause)
            {
                tutorialImage.sprite = tutorialSprite[3];
            }
            tutorialBackgroundImg.GetComponent<Animator>().SetBool("FadeIn", true);
            isFullShow = true;
            Invoke(nameof(NoMoreTuto), showTimer);
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
