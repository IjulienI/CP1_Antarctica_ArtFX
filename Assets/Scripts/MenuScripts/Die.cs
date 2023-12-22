using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    [SerializeField] private Button retryBtn;

    private void Start()
    {
        retryBtn.Select();
    }
    public void Retry()
    {
        SceneManager.LoadScene("");
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
