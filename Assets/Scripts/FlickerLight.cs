using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    [SerializeField] private Light2D light1;
    [SerializeField] private Light2D light2;
    public float maxInterval = 2;
    public float maxBurst = 1f;
    public float maxFlicker = 0.2f;
    float defaultIntensity1;
    float defaultIntensity2;
    bool isFlickering;
    float timer;
    float interval;

    [SerializeField] private ParticleSystem sparks;

    private void Start()
    {
        defaultIntensity1 = light1.intensity;
        defaultIntensity2 = light2.intensity;
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
            sparks.Play();
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
                light1.intensity = Random.Range(0, defaultIntensity1);
                light2.intensity = Random.Range(0, defaultIntensity2);
                flickerTimer = 0;
                
            }
            yield return null;
        }
        light1.intensity = defaultIntensity1;
        light2.intensity = defaultIntensity2;
        isFlickering = false;
    }
}
