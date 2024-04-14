using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float lifetime = 5.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}

