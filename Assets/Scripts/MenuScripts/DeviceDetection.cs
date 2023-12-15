using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceDetection : MonoBehaviour
{
    public static DeviceDetection instance;

    private bool isKeyboard;
    private bool isGamepad;
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void DeviceDetector(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device is Keyboard || ctx.control.device is Mouse)
        {
            isGamepad = false;
            isKeyboard = true;
        }
        else if (ctx.control.device is Gamepad)
        {
            isKeyboard = false;
            isGamepad = true;
        }
    }
    public bool GetIsKeyboard() { return isKeyboard; }
    public bool GetIsGamepad() { return isGamepad; }
}
