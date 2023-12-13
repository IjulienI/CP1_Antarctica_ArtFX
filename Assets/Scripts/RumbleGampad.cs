using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleGamepad : MonoBehaviour
{
    public static RumbleGamepad instance;
    private bool playing = false;

    private void Start()
    {
        instance = this;
    }
    public void MakeGampadRumble(float lowFrequency, float highFrequency, float rumbleDuration)
    {
        if (Gamepad.current != null && !playing)
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

