using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenFade : MonoBehaviour
{
    private Image deathScreenImage;
    public float fadeSpeed = 0.8f;
    private bool isDead = false;

    private void Start()
    {
        deathScreenImage = GetComponent<Image>();
        deathScreenImage.color = new Color(deathScreenImage.color.r, deathScreenImage.color.g, deathScreenImage.color.b, 0);
    }

    private void Update()
    {
        if (isDead && Input.GetMouseButtonDown(0))
        {
            ResetScene();
        }
    }

    public void PlayerDied()
    {
        if (!isDead)
        {
            StartCoroutine(FadeInDeathScreen());
        }
    }

    private IEnumerator FadeInDeathScreen()
    {
        float alphaValue = 0;

        while (alphaValue < 1)
        {
            alphaValue += Time.deltaTime / fadeSpeed;
            deathScreenImage.color = new Color(deathScreenImage.color.r, deathScreenImage.color.g, deathScreenImage.color.b, alphaValue);
            yield return null;
        }

        isDead = true;
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
