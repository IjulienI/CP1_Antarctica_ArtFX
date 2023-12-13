using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleGamepad : MonoBehaviour
{
    public static RumbleGamepad instance;

    private void Start()
    {
        instance = this;
    }
    public void MakeGampadRumble(float lowFrequency, float highFrequency, float rumbleDuration)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.2f, 1f);
            Invoke(nameof(StopRumble), rumbleDuration);
        }
    }
    private void StopRumble()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
}

