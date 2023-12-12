using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menus Canvas")]
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas optionMenuCanvas;
    [SerializeField] private Canvas creditsMenuCanvas;
    [Header("Buttons")]
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button volumeBtn;
    [SerializeField] private Button keybindsBtn;
    [SerializeField] private Button graficsBtn;
    [Header("Volume Slider")]
    [SerializeField] private Slider globalVolumeSlider;
    [Header("Options Menus Backgrounds")]
    [SerializeField] private Image volumeImg;
    [SerializeField] private Image keybindsImg;
    [SerializeField] private Image graphicsImg;
    private bool isInOptions, isInCredits, isInVolume, isInKeybinds, isInGraphics;
    [Header("Keybinds References")]
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
        graphicsImg.gameObject.SetActive(false);
    }
    public void Keybinds()
    {
        isInKeybinds = true;
        volumeImg.gameObject.SetActive(false);
        keybindsImg.gameObject.SetActive(true);
        graphicsImg.gameObject.SetActive(false);
    }
    public void Graphics()
    {
        isInGraphics = true;
        volumeImg.gameObject.SetActive(false);
        keybindsImg.gameObject.SetActive(false);
        graphicsImg.gameObject.SetActive(true);
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
        isInGraphics = false;
        graphicsImg.gameObject.SetActive(false);
    }

    private void ReturnBack(InputAction.CallbackContext obj)
    {
        if (isInOptions && (!isInVolume && !isInKeybinds && !isInGraphics))
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
        if (isInGraphics && isInOptions)
        {
            ExitGraphics();
        }
    }
}
