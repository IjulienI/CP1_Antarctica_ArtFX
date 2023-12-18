using System.Collections;
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
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(InvokeStopRumble(rumbleDuration));
        }
    }
    private IEnumerator InvokeStopRumble(float rumbleDuration)
    {
        yield return new WaitForSecondsRealtime(rumbleDuration);
        StopRumble();
    }
    private void StopRumble()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
        playing = false;
    }
}

