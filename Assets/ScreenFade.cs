using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Make sure to include the UI namespace

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage; // Reference to the Image component
    private CanvasGroup canvasGroup; // Reference to the CanvasGroup

    void Awake()
    {
        // Get the CanvasGroup component to control transparency of the entire UI
        canvasGroup = GetComponent<CanvasGroup>();

        // If there's no CanvasGroup attached, create one dynamically (optional)
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        // Ensure the screen is invisible on start
        SetAlpha(0f);
    }

    public void FadeIn(float duration)
    {
        // Fade in to make the screen black
        StartCoroutine(FadeCoroutine(1f, duration));  // Fade to 1 alpha (fully opaque)
    }

    public void FadeOut(float duration)
    {
        // Fade out to make the screen invisible
        StartCoroutine(FadeCoroutine(0f, duration));  // Fade to 0 alpha (fully transparent)
    }

    private IEnumerator FadeCoroutine(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            yield return null;
        }

        // Ensure the final alpha is exactly the target value
        canvasGroup.alpha = targetAlpha;
    }

    // Helper method to set alpha directly (useful for Start())
    private void SetAlpha(float alpha)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            Debug.LogError("CanvasGroup is missing on the ScreenFade GameObject.");
        }
    }
}
