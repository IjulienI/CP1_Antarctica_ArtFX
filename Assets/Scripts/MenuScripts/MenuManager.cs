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
    private bool isInOptions, isInCredits;
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
    }
}
