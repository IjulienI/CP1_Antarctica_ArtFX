using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FirstSceneVideoPlayer : MonoBehaviour
{
    private VideoPlayer audioSource;
    private bool delay;

    private void Start()
    {
        audioSource = GetComponent<VideoPlayer>();
    }
    void Update()
    {
        if(audioSource.isPlaying && !delay)
        {
            delay = true;
        }
        if(!audioSource.isPlaying && delay)
        {
            if(File.Exists(Application.persistentDataPath + "/data.save"))
            {
                File.Delete(Application.persistentDataPath + "/data.save");
            }
            SceneManager.LoadScene("SaveSystem");
        }
    }
}
