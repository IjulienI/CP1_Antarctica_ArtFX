using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas, optionMenuCanvas, creditsMenuCanvas;
    [SerializeField] private Button newGameBtn, volumeBtn, keybindsBtn, graficsBtn;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Image volumeImg, keybindsImg, graficsImg;
    private bool isInOptions, isInCredits, isInVolume, isInKeybinds, isInGrafics;
    [SerializeField] private InputActionReference escape;
    private void OnEnable()
    {
        escape.action.started += ReturnBack;
    }

    private void OnDisable()
    {
        escape.action.started -= ReturnBack;
    }

    public void NewGame()
    {
        print("test");
    }
    public void Continue()
    {
        print("test1");
    }
    public void Option()
    {
        isInOptions = true;
        volumeBtn.Select();
        mainMenuCanvas.gameObject.SetActive(false);
        optionMenuCanvas.gameObject.SetActive(true);
    }
    public void Credits()
    {
        isInCredits = true;
        mainMenuCanvas.gameObject.SetActive(false);
        creditsMenuCanvas.gameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ExitOptionsMenu()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        optionMenuCanvas.gameObject.SetActive(false);
        newGameBtn.Select();
    }
    public void ExitCreditsMenu() 
    {
        mainMenuCanvas.gameObject.SetActive(true);
        creditsMenuCanvas.gameObject.SetActive(false);
        newGameBtn.Select();
    }
    public void Volume()
    {
        isInVolume = true;
        globalVolumeSlider.Select();
        volumeImg.gameObject.SetActive(true);
        keybindsImg.gameObject.SetActive(false);
        graficsImg.gameObject.SetActive(false);
    }
    public void Keybinds()
    {
        isInKeybinds = true;
        volumeImg.gameObject.SetActive(false);
        keybindsImg.gameObject.SetActive(true);
        graficsImg.gameObject.SetActive(false);
    }
    public void Graphics()
    {
        isInGrafics = true;
        volumeImg.gameObject.SetActive(false);
        keybindsImg.gameObject.SetActive(false);
        graficsImg.gameObject.SetActive(true);
    }
    public void ExitVolume()
    {
        volumeBtn.Select();
        isInVolume = false;
        volumeImg.gameObject.SetActive(false);
    }
    public void ExitKeybinds()
    {
        keybindsBtn.Select();
        isInKeybinds = false;
        keybindsImg.gameObject.SetActive(false);
    }
    public void ExitGraphics()
    {
        graficsBtn.Select();
        isInGrafics = false;
        graficsImg.gameObject.SetActive(false);
    }

    private void ReturnBack(InputAction.CallbackContext obj)
    {
        if (isInOptions && (!isInVolume && !isInKeybinds && !isInGrafics))
        {
            ExitOptionsMenu();
        }
        if (isInCredits)
        {
            ExitCreditsMenu();
        }
        if (isInVolume && isInOptions)
        {
            ExitVolume();
        }
        if (isInKeybinds && isInOptions)
        {
            ExitKeybinds();
        }
        if (isInGrafics && isInOptions)
        {
            ExitGraphics();
        }
    }
}
