using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleGamepad : MonoBehaviour
{
    public static RumbleGamepad instance;
    private bool playing = false;
    private int isActivated;

    private void Start()
    {
        instance = this;
    }
    public void MakeGampadRumble(float lowFrequency, float highFrequency, float rumbleDuration)
    {
        isActivated = PlayerPrefs.GetInt("Vibration activation", 1);
        if (Gamepad.current != null && !playing && isActivated == 1)
        {
            playing = true;
            Gamepad.current.SetMotorSpeeds(0.2f, 1f);
            Invoke(nameof(StopRumble), rumbleDuration);
        }
    }
    private void StopRumble()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
        playing = false;
    }
}

