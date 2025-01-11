using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class WallManager : MonoBehaviour
{
    [Header("Walls")]
    [Tooltip("The original wall to be replaced.")]
    [SerializeField] private GameObject oldWall;

    [Tooltip("The new wall that replaces the old one.")]
    [SerializeField] private GameObject newWall;

    [Header("Fade Settings")]
    [Tooltip("Image used for fade effect.")]
    [SerializeField] private Image fadeImage;

    [Tooltip("Duration of the fade effect in seconds.")]
    [SerializeField] private float fadeDuration = 1.0f;

    private void OnEnable()
    {
        DialogueSystem.DialogueEvents.wallEvent += StartFadeAndChangeAssets;
    }

    private void OnDisable()
    {
        DialogueSystem.DialogueEvents.wallEvent -= StartFadeAndChangeAssets;
    }

    private void ReplaceWall()
    {
        if (oldWall != null && newWall != null)
        {
            oldWall.SetActive(false);
            newWall.SetActive(true);
            Debug.Log("El muro se ha transformado con éxito.");
        }
        else
        {
            Debug.LogWarning("No se han asignado los muros en el WallManager.");
        }
    }

    public void StartFadeAndChangeAssets()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade image is not assigned in WallManager.");
            return;
        }

        StartCoroutine(FadeOutAndIn());
    }

    private IEnumerator FadeOutAndIn()
    {
        yield return Fade(1); // Fade out
        ReplaceWall();
        yield return Fade(0); // Fade in
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break;

        float startAlpha = fadeImage.color.a;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            SetImageAlpha(newAlpha);
            yield return null;
        }

        SetImageAlpha(targetAlpha);
    }

    private void SetImageAlpha(float alpha)
    {
        if (fadeImage == null) return;

        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
