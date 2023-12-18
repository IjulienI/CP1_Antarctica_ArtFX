using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera Vcam;
    private bool shakeStarted = false;

    private void Start()
    {
        Vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        if(!shakeStarted)
        {
            shakeStarted = true;
            Vector3 originalPos = Vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

                Vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(xOffset, yOffset + 2, 0);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            Vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = originalPos;
            shakeStarted = false;
        }
    }
}
