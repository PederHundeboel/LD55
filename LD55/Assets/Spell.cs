using UnityEngine;

public class Spell : MonoBehaviour
{
    public Vector2 boxSize = new Vector2(0.5f, 0.5f);
    public Vector2 boxOffset = Vector2.zero;
    public AudioClip castSound;

    public void Cast()
    {
        CheckCollisions();
        AudioController.Instance.PlayOneShotAudioClip(castSound, transform.position);
    }

    private void CheckCollisions()
    {
        Vector2 boxCenter = (Vector2)transform.position + boxOffset;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0);

        foreach (Collider2D hitCollider in hitColliders)
        {
            HealthContainer health = hitCollider.GetComponent<HealthContainer>();
            if (health?.gameObject == this.transform.parent.gameObject) continue;
            if (health != null)
            {
                ApplyEffect(health);
            }
        }
    }

    private void ApplyEffect(HealthContainer health)
    {
        health.Subtract(1);
    }
}
