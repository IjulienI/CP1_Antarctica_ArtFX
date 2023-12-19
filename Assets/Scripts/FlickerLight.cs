using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    public Light2D myLight;
    public float maxInterval = 2;
    public float maxBurst = 1f;
    public float maxFlicker = 0.2f;
    float defaultIntensity;
    bool isFlickering;
    float timer;
    float interval;
    private void Start()
    {
        defaultIntensity = myLight.intensity;
        interval = Random.Range(0, maxInterval);
    }
    void Update()
    {
        if (!isFlickering)
        {
            timer += Time.deltaTime;
        }
        if (timer > interval)
        {
            interval = Random.Range(0, maxInterval);
            timer = 0;
            StartCoroutine(DoFlickerLight(Random.Range(0, maxBurst)));
        }
    }
    IEnumerator DoFlickerLight(float duration)
    {
        isFlickering = true;
        float totalTime = 0;
        float flickerTimer = 0;
        while (totalTime < duration)
        {
            totalTime += Time.deltaTime;
            flickerTimer += Time.deltaTime;
            if (flickerTimer > 0.1f)
            {
                myLight.intensity = Random.Range(0, defaultIntensity);
                flickerTimer = 0;
            }
            yield return null;
        }
        myLight.intensity = defaultIntensity;
        isFlickering = false;
    }
}
