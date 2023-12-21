using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorChangeScene : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Level2");
    }
}
