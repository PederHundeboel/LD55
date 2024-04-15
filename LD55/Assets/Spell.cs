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

    //this spell is only used by enemies, and as such they should only hit player types
    private void CheckCollisions()
    {
        Vector2 boxCenter = (Vector2)transform.position + boxOffset;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Player player = hitCollider.GetComponent<Player>();
            if (player != null)
            {
                var playerHealth = player.GetComponent<HealthContainer>();
                ApplyEffect(playerHealth);
            }
        }
    }

    private void ApplyEffect(HealthContainer health)
    {
        health.Subtract(1);
    }
}
