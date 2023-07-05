using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    private void Start()
    {
        fadeCanvasGroup.alpha = 0f;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(0f, 1f));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(1f, 0f));
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;

        if (endAlpha == 0f)
        {
            SceneManager.LoadScene("LevelManager");
            FadeIn();
        }
    }
}
