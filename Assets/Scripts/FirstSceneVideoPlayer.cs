using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneVideoPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            print("is");
            SceneManager.LoadScene("");
        }
    }
}
