using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NumpadDoor : MonoBehaviour
{
    [SerializeField] private GameObject keybindGo;
    [SerializeField] private Image numpadImg;
    [Header("Numpad buttons")]
    private Button[] numpadButtons = new Button[10];
    [SerializeField] private Button numpadValidateBtn, numpadDeleteBtn;
    [Header("Keybinds References")]
    [SerializeField] private InputActionReference escape;
    [SerializeField] private InputActionReference interract;
    [Header("Keybind Sprites")]
    [SerializeField] private Sprite keyboardKeybind;
    [SerializeField] private Sprite gamepadKeybind;
    [Header("Gamepad Rumble Numpad Key")]
    [SerializeField] private float lowFrequencyNumpadKey = 0.5f;
    [SerializeField] private float highFrequencyNumpadKey = 0.5f;
    [SerializeField] private float rumbleDurationNumpadKey = 0.5f;
    [Header("Gamepad Rumble Wrong code")]
    [SerializeField] private float lowFrequencyWrongCode = 0.5f;
    [SerializeField] private float highFrequencyWrongCode = 0.5f;
    [SerializeField] private float rumbleDurationWrongCode = 0.5f;

    private bool isSelectButtonShowed;
    private bool isNumpadShowed;
    private bool isDoorOpened;
    private int childCount;
    int count = 0;


    [Header("Code de la porte (svp 4 chiffres max)")]
    [SerializeField]private List<int> code = new List<int>();
    private List<int> codePlayer = new List<int>();
    [SerializeField] private TextMeshProUGUI[] numberText = new TextMeshProUGUI[4];

    private void Awake()
    {
        childCount = numpadImg.transform.childCount;
        for (int i = 1; i < childCount-3; i++)
        {
            numpadButtons[i-1] = numpadImg.transform.GetChild(i).GetComponent<Button>();
        }
    }

    private void Start()
    {
        numpadButtons[0].onClick.AddListener(() => addNumberInCode(0));
        numpadButtons[1].onClick.AddListener(() => addNumberInCode(1));
        numpadButtons[2].onClick.AddListener(() => addNumberInCode(2));
        numpadButtons[3].onClick.AddListener(() => addNumberInCode(3));
        numpadButtons[4].onClick.AddListener(() => addNumberInCode(4));
        numpadButtons[5].onClick.AddListener(() => addNumberInCode(5));
        numpadButtons[6].onClick.AddListener(() => addNumberInCode(6));
        numpadButtons[7].onClick.AddListener(() => addNumberInCode(7));
        numpadButtons[8].onClick.AddListener(() => addNumberInCode(8));
        numpadButtons[9].onClick.AddListener(() => addNumberInCode(9));
        numpadValidateBtn.onClick.AddListener(CodeValidate);
        numpadDeleteBtn.onClick.AddListener(CodeClear);
    }
    private void OnEnable()
    {
        escape.action.started += ReturnBack;
        interract.action.started += Interract;
    }

    private void OnDisable()
    {
        escape.action.started -= ReturnBack;
        interract.action.started -= Interract;
    }
    private void Interract(InputAction.CallbackContext context)
    {
        if (isSelectButtonShowed && !isNumpadShowed)
        {
            PlayerMovement.instance.enabled = false;
            PlayerMovement.instance.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            PlayerMovement.instance.SetCanJump(false);
            ResetCodeText("-");
            numpadImg.gameObject.SetActive(true);
            keybindGo.SetActive(false);
            isNumpadShowed = true;
            numpadButtons[1].Select();
            if (DeviceDetection.instance.GetIsGamepad())
            {
                keybindGo.GetComponent<SpriteRenderer>().sprite = gamepadKeybind;
            }
        }
        else if(DeviceDetection.instance.GetIsKeyboard())
        {
            codePlayer.Clear();
            keybindGo.GetComponent<SpriteRenderer>().sprite = keyboardKeybind;
            count = 0;
            PlayerMovement.instance.enabled = true;
            PlayerMovement.instance.SetCanJump(true);
            numpadImg.gameObject.SetActive(false);
            keybindGo.SetActive(true);
            isNumpadShowed = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDoorOpened) 
        {
            if (DeviceDetection.instance.GetIsKeyboard())
            {
                keybindGo.GetComponent<SpriteRenderer>().sprite = keyboardKeybind;
                
            }
            else if (DeviceDetection.instance.GetIsGamepad())
            {
                keybindGo.GetComponent<SpriteRenderer>().sprite = gamepadKeybind;
            }
            keybindGo.SetActive(true);
            isSelectButtonShowed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            keybindGo.SetActive(false);
            isSelectButtonShowed = false;
        }
    }
    private void ReturnBack(InputAction.CallbackContext obj)
    {
        if (isNumpadShowed && !isDoorOpened)
        {
            codePlayer.Clear();
            count = 0;
            PlayerMovement.instance.enabled = true;
            PlayerMovement.instance.SetCanJump(true);
            numpadImg.gameObject.SetActive(false);
            keybindGo.SetActive(true);
            isNumpadShowed = false;
        }
    }
    private void addNumberInCode(int number)
    {
        if (codePlayer.Count < 4)
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyNumpadKey, highFrequencyNumpadKey, rumbleDurationNumpadKey);
            codePlayer.Add(number);
            numberText[count].SetText("" + number);
            count++;
        }
    }
    private void CodeClear()
    {
        count = 0;
        foreach (TextMeshProUGUI numberTxt in numberText)
        {
            numberTxt.color = Color.blue;
        }
        ResetCodeText("-");
        codePlayer.Clear();
    }
    private void CodeValidate()
    {
        count = 0;
        if (code.SequenceEqual(codePlayer))
        {
            foreach (TextMeshProUGUI numberTxt in numberText)
            {
                numberTxt.color = Color.green;
            }
            Invoke(nameof(OpenDoor), 1f);
        }
        else
        {
            RumbleGamepad.instance.MakeGampadRumble(lowFrequencyWrongCode, highFrequencyWrongCode, rumbleDurationWrongCode);
            foreach (TextMeshProUGUI numberTxt in numberText)
            {
                numberTxt.color = Color.red;
                Invoke(nameof(CodeClear), 1f);
            } 
        }
    }
    private void ResetCodeText(string text)
    {
        foreach (TextMeshProUGUI numberTxt in numberText)
        {
            numberTxt.SetText(text);
        }
    }
    private void OpenDoor()
    {
        isDoorOpened = true;
        numpadImg.gameObject.SetActive(false);
        PlayerMovement.instance.enabled = true;
        PlayerMovement.instance.SetCanJump(true);
    }
    
}
