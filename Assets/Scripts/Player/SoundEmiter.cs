using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmiter : MonoBehaviour
{
    FieldOfView _field;
    private void Awake()
    {
        _field = GameObject.FindAnyObjectByType<FieldOfView>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            MakeNoise();
        }
    }
    public void MakeNoise()
    {
        _field.ReceiveNoise();
    }
}
