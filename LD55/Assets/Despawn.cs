using UnityEngine;
using UnityEngine.Events;

public class Despawn : MonoBehaviour
{
    public float lifetime = 5.0f;
    public UnityEvent damage = new UnityEvent();

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnDestroy()
    {
        damage.Invoke();
        Destroy(gameObject);
    }
}

