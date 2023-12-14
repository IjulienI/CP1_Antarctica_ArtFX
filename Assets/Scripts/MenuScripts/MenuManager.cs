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
    [SerializeField] private Button generalBtn;
    [Header("Volume Slider")]
    [SerializeField] private Slider globalVolumeSlider;
    [Header("General Options Toggles")]
    [SerializeField] private Toggle gamepadVibrationsToggle;
    [SerializeField] private Toggle fullscreenToggle;
    [Header("Options Menus Backgrounds")]
    [SerializeField] private Image volumeImg;
    [SerializeField] private Image keybindsImg;
    [SerializeField] private Image generalImg;
    private bool isInOptions, isInCredits, isInVolume, isInKeybinds, isInGeneral;
    [Header("Keybinds References")]
    [SerializeField] private InputActionReference escape;
    [Header("Gamepad Rumble settings - Return Button (For the frequencies, 1 is the max value)")]
    [SerializeField] private float lowFrequencyReturnButton;
    [SerializeField] private float highFrequencyReturnButton;
    [SerializeField] private float rumbleDurationReturnButton;
    [Header("Gamepad Rumble settings - Volume Slider (For the frequencies, 1 is the max value)")]
    [SerializeField] private float lowFrequencyVolumeSlider;
    [SerializeField] private float highFrequencyVolumeSlider;
    [SerializeField] private float rumbleDurationVolumeSlider;

    public static MenuManager instance;

    private bool hasLoad;
    private int isVibrationsActivated;
    private void OnEnable()
    {
        escape.action.started += ReturnBack;
    }

    private void OnDisable()
    {
        escape.action.started -= ReturnBack;
        PlayerPrefs.SetInt("Vibration activation", isVibrationsActivated);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        isVibrationsActivated = PlayerPrefs.GetInt("Vibration activation", 1);
        if (isVibrationsActivated == 1)
        {
            gamepadVibrationsToggle.isOn = true;
        }
        else if (isVibrationsActivated == 0)
        {
            gamepadVibrationsToggle.isOn = false;
        }
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
        generalBtn.Select();
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
        isInOptions = false;
    }
    public void ExitCreditsMenu() 
    {
        mainMenuCanvas.gameObject.SetActive(true);
        creditsMenuCanvas.gameObject.SetActive(false);
        newGameBtn.Select();
        isInCredits = false;
    }
    public void Volume()
    {
        isInVolume = true;
        globalVolumeSlider.Select();
        volumeImg.gameObject.SetActive(true);
        keybindsImg.gameObject.SetActive(false);
        generalImg.gameObject.SetActive(false);
    }
    public void Keybinds()
    {
        isInKeybinds = true;
        volumeImg.gameObject.SetActive(false);
        keybindsImg.gameObject.SetActive(true);
        generalImg.gameObject.SetActive(false);
    }
    public void General()
    {
        isInGeneral = true;
        gamepadVibrationsToggle.Select();
        volumeImg.gameObject.SetActive(false);
        keybindsImg.gameObject.SetActive(false);
        generalImg.gameObject.SetActive(true);
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
        generalBtn.Select();
        isInGeneral = false;
        generalImg.gameObject.SetActive(false);
    }

    private void ReturnBack(InputAction.CallbackContext obj)
    {
        if (isInOptions && (!isInVolume && !isInKeybinds && !isInGeneral))
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyReturnButton, highFrequencyReturnButton, rumbleDurationReturnButton);
            ExitOptionsMenu();
        }
        if (isInCredits)
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyReturnButton, highFrequencyReturnButton, rumbleDurationReturnButton);
            ExitCreditsMenu();
        }
        if (isInVolume && isInOptions)
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyReturnButton, highFrequencyReturnButton, rumbleDurationReturnButton);
            ExitVolume();
        }
        if (isInKeybinds && isInOptions)
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyReturnButton, highFrequencyReturnButton, rumbleDurationReturnButton);
            ExitKeybinds();
        }
        if (isInGeneral && isInOptions)
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyReturnButton, highFrequencyReturnButton, rumbleDurationReturnButton);
            ExitGraphics();
        }
    }
    public void ChangeValueSliders()
    {
        if (hasLoad)
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyVolumeSlider, highFrequencyVolumeSlider, rumbleDurationVolumeSlider);
        }
    }
    public void GamepadVibrations()
    {
        if (isVibrationsActivated == 1 && hasLoad)
        {
            isVibrationsActivated = 0;
            PlayerPrefs.SetInt("Vibration activation",isVibrationsActivated);
        }
        else if(isVibrationsActivated == 0 && hasLoad)
        {
            isVibrationsActivated = 1;
            PlayerPrefs.SetInt("Vibration activation", isVibrationsActivated);
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyVolumeSlider, highFrequencyVolumeSlider, rumbleDurationVolumeSlider);
        }
    }

    public void setFullscreen()
    {
        if (fullscreenToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyVolumeSlider, highFrequencyVolumeSlider, rumbleDurationVolumeSlider);
        }

        else 
        { 
            Screen.fullScreenMode = FullScreenMode.Windowed;
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyVolumeSlider, highFrequencyVolumeSlider, rumbleDurationVolumeSlider);
        }
    }


    public void setHasLoad()
    {
        hasLoad = true;
    }
}
