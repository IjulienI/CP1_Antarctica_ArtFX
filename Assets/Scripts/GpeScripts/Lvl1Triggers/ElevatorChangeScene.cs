using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ElevatorChangeScene : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void GamepadRumble()
    {
        RumbleGamepad.instance.MakeGampadRumble(0.2f, 0.2f, 10);
        Invoke(nameof(SmallRumble), 10.5f);
    }
    private void SmallRumble()
    {
        RumbleGamepad.instance.MakeGampadRumble(0.1f, 0.2f, 15);
    }
    private void OnDisable()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}
