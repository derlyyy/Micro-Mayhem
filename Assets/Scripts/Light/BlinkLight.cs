using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlinkLight : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D lightToBlink;
    public float minBlinkInterval = 0.1f;
    public float maxBlinkInterval = 1.0f;
    public float minIntensity = 0f;
    public float maxIntensity = 1f; 

    [Header("Intensity Settings")]
    public float intensityChangeDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true) 
        {
            float blinkInterval = Random.Range(minBlinkInterval, maxBlinkInterval);
            float blinkDuration = Random.Range(0.1f, 0.5f);

            yield return StartCoroutine(ChangeIntensity(lightToBlink.intensity, maxIntensity, intensityChangeDuration));

            yield return new WaitForSeconds(blinkDuration);

            yield return StartCoroutine(ChangeIntensity(lightToBlink.intensity, minIntensity, intensityChangeDuration));

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator ChangeIntensity(float startIntensity, float endIntensity, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            lightToBlink.intensity = Mathf.Lerp(startIntensity, endIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        lightToBlink.intensity = endIntensity; 
    }
}
