using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GlowingEyes : MonoBehaviour
{
    [SerializeField] private Image targetImage;  // The UI Image to fade
    [SerializeField] private float duration = 5f;  // Duration of one fade (in or out)
    [SerializeField] private float minAlpha = 0f;  // Minimum alpha value
    [SerializeField] private float maxAlpha = 0.56f;  // Maximum alpha value

    private void Start()
    {
        if (targetImage != null)
        {
            // Start the fade loop
            StartCoroutine(FadeImageInOut());
        }
    }

    private IEnumerator FadeImageInOut()
    {
        while (true)
        {
            // Fade to minAlpha
            yield return StartCoroutine(Fade(maxAlpha, minAlpha));
            // Fade to maxAlpha
            yield return StartCoroutine(Fade(minAlpha, maxAlpha));
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = targetImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            targetImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure the final alpha value is set
        targetImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
