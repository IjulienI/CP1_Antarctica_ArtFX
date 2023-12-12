using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas, optionMenuCanvas, creditsMenuCanvas;
    [SerializeField] private Button newGameBtn, exitOptionBtn;
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
        exitOptionBtn.Select();
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
        volumeImg.gameObject.SetActive(true);
    }
    public void Keybinds()
    {
        isInKeybinds = true;
        keybindsImg.gameObject.SetActive(true);
    }
    public void Graphics()
    {
        isInGrafics = true;
        graficsImg.gameObject.SetActive(true);
    }
    public void ExitVolume()
    {
        isInVolume = false;
        volumeImg.gameObject.SetActive(false);
    }
    public void ExitKeybinds()
    {
        isInKeybinds = false;
        keybindsImg.gameObject.SetActive(false);
    }
    public void ExitGraphics()
    {
        isInGrafics = false;
        graficsImg.gameObject.SetActive(false);
    }

    private void ReturnBack(InputAction.CallbackContext obj)
    {
        if (isInOptions)
        {
            ExitOptionsMenu();
        }
        if (isInCredits)
        {
            ExitCreditsMenu();
        }
        if (isInVolume)
        {
            ExitVolume();
        }
        if (isInKeybinds)
        {
            ExitKeybinds();
        }
        if (isInGrafics)
        {
            ExitGraphics();
        }
    }
}
