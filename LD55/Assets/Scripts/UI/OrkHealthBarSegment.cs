using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrkHealtBarSegment : MonoBehaviour
{
    [SerializeField] public Sprite fullHealthSprite;
    [SerializeField] private Sprite emptyHealthSprite;
    [SerializeField] private Sprite decreaseSprite;
    [SerializeField] private float blinkDelay = 0.5f;

    private SpriteRenderer healthUiImage;
    private bool previousDisplayHealth = true;

    private void Awake()
    {
        healthUiImage = GetComponent<SpriteRenderer>();
    }

    public void SetSortOrder(int sortOrder)
    {
        healthUiImage.sortingOrder = sortOrder;
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