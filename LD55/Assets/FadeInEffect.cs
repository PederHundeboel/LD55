using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f;
    public bool fadeInOnStart = true;

    private void Start()
    {
        if (fadeInOnStart)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float alphaValue = fadeOutUIImage.color.a;

        while (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime / fadeSpeed;
            Color newColor = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alphaValue);
            fadeOutUIImage.color = newColor;
            yield return null;
        }
    }
}

