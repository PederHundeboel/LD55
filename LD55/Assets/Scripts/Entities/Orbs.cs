using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Orbs : Container
{

    public Orb OrbPrefab;
    public Player Target;
    public float InnerRadius = 0.5f;
    public float OuterRadius = 1.5f;

    private List<Orb> _orbs = new ();
    private float angleOffset = 0;
    private int _targetSortOrder => Target.GetSortOrder();
    
    private float soundDelay = 1.387f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < value; i++)
        {
            CreateOrb();
        }

        onValueChange.AddListener(UpdateOrbs);

    }

    private void UpdateOrbs()
    {
        // If there are more orbs than needed, remove the excess
        while (_orbs.Count > value)
        {
            RemoveOrb();
        }

        // If there are less orbs than needed, add the missing ones
        while (_orbs.Count < value)
        {
            CreateOrb();
        }
    }

    private void RemoveOrb()
    {
        if (_orbs.Count == 0)
        {
            Debug.LogError("No orbs to remove");
            return;
        }
        
        var orb = _orbs.First();
        _orbs.Remove(orb);
        Destroy(orb);
    }

    public void CreateOrb(int index = 0)
    {
        var orb = Instantiate(OrbPrefab, transform);
        orb.transform.localPosition = Vector3.zero;

        _orbs.Add(orb);
    }

    public Orb GetPassiveOrb()
    {
        if (value <= 0)
        {
            return null;
        }

        var orb = _orbs.First(o => !o.IsActive);
        if (orb)
        {
            return orb;
        }
        return null;
    }
    
    public bool HasPassiveOrb()
    {
        return _orbs.Any(o => !o.IsActive);
    }

    public Dictionary<SpellResources.SpellType, int> ConsumeOrbs(Action onConsume = null, Action onCast = null)
    {
        var consumedOrbs = _orbs.Where(o => o.IsActive).ToList();
        Dictionary<SpellResources.SpellType, int> consumedValues = new();
        foreach (var orb in consumedOrbs)
        {
            if (orb.IsActive)
            {
                if (consumedValues.ContainsKey(orb.Type))
                {
                    consumedValues[orb.Type]++;
                }
                else
                {
                    consumedValues[orb.Type] = 1;
                }
            }
        }
        onConsume?.Invoke();
        ShakeOrbs(consumedOrbs, onCast);

        return consumedValues;
    }

    private void ShakeOrbs(List<Orb> consumedOrbs, Action onCast)
    {
        StartCoroutine(JitterOrbs(consumedOrbs, soundDelay, onCast));
    }
    
    private IEnumerator JitterOrbs(List<Orb> orbs, float duration, Action onCast)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            foreach (var orb in orbs)
            {
                orb.transform.position += new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
            }
            yield return null;
        }

        foreach (var orb in orbs)
        {
            orb.SetDefault();
        }
        onCast?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        angleOffset += Time.deltaTime;
        float offset = 2 * Mathf.PI / _orbs.Count;

        foreach (Orb orb in _orbs)
        {
            float angle = angleOffset + _orbs.IndexOf(orb) * offset;
            float radius = orb.IsActive ? InnerRadius : OuterRadius;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector2 targetPosition = Target.transform.position + new Vector3(x, y, 0);
            orb.transform.position = Vector3.Lerp(orb.transform.position, targetPosition, 0.1f);

            SpriteRenderer spriteRenderer = orb.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                spriteRenderer.sortingOrder = orb.transform.position.y > Target.transform.position.y ? _targetSortOrder-1 : _targetSortOrder+1;
            }
        }
    }
}
