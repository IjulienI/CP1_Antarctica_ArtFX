using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class NumpadDoor : MonoBehaviour
{
    [SerializeField] private GameObject keybindGo;
    [SerializeField] private Image numpadImg;
    [Header("Numpad buttons")]
    private Button[] numpadButtons = new Button[10];
    [SerializeField] private Button numpadValidateBtn, numpadDeleteBtn;
    [Header("Keybinds References")]
    [SerializeField] private InputActionReference escape;

    private bool isSelectButtonShowed;
    private bool isNumpadShowed;
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
    }

    private void OnDisable()
    {
        escape.action.started -= ReturnBack;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isSelectButtonShowed && !isNumpadShowed)
        {
            codePlayer.Clear();
            ResetCodeText("-");
            numpadImg.gameObject.SetActive(true);
            keybindGo.SetActive(false);
            isNumpadShowed = true;
            numpadButtons[1].Select();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
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
        if (isNumpadShowed)
        {
            numpadImg.gameObject.SetActive(false);
            keybindGo.SetActive(true);
            isNumpadShowed = false;
        }
    }
    private void addNumberInCode(int number)
    {
        if (codePlayer.Count < 4)
        {
            print(count);
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
            print("reussi");
        }
        else
        {   
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
}
