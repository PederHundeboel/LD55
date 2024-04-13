using System.Collections.Generic;
using UnityEngine;


public class Healthbar : MonoBehaviour
{
    [SerializeField]
    private Container healthContainer;
    [SerializeField]
    private List<HealthChunk> healthChunks;

    private void Awake()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        int value = healthContainer.GetValue();
        int chunksPerUnit = healthContainer.GetMax() / healthChunks.Count;

        for (int i = 0; i < healthChunks.Count; i++)
        {
            healthChunks[i].SetDisplayHealth((i + 1) * chunksPerUnit <= value);
        }
    }
}
