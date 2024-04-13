using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContainerBarSegment : MonoBehaviour
{
    [SerializeField] private Sprite fullHealthSprite;
    [SerializeField] private Sprite emptyHealthSprite;
    [SerializeField] private Sprite decreaseSprite;
    [SerializeField] private float blinkDelay = 0.5f;

    private Image healthUiImage;
    private bool previousDisplayHealth = true;

    private void Awake()
    {
        healthUiImage = GetComponent<Image>();
    }

    public void SetDisplayHealth(bool displayHealth)
    {
        if (displayHealth)
        {
            healthUiImage.sprite = fullHealthSprite;
        }
        else
        {
            if (previousDisplayHealth)
            {
                StartCoroutine(BlinkDecreaseSprite());
            }
            else
            {
                healthUiImage.sprite = emptyHealthSprite;
            }
        }
        previousDisplayHealth = displayHealth;
    }

    private IEnumerator BlinkDecreaseSprite()
    {
        healthUiImage.sprite = decreaseSprite;
        yield return new WaitForSeconds(blinkDelay);
        healthUiImage.sprite = emptyHealthSprite;
    }
}
