using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Orbs : Container
{

    public List<GameObject> OrbPrefabs;
    public GameObject Target;
    public float Radius = 1;

    private List<GameObject> _orbs = new List<GameObject>();
    private Dictionary<GameObject, float> _activeOrbCooldowns = new Dictionary<GameObject, float>();
    private float angleOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < value; i++)
        {
            CreateOrb();
        }

        onChange.AddListener(UpdateOrbs);

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

        GameObject orb = _orbs.First();
        _orbs.Remove(orb);
        Destroy(orb);
    }

    public void CreateOrb(int index = 0)
    {
        if (index >= OrbPrefabs.Count)
        {
            Debug.LogError("Orb index out of range");
            return;
        }

        GameObject orb = Instantiate(OrbPrefabs[index], transform);
        orb.transform.localPosition = Vector3.zero;

        _orbs.Add(orb);
        _activeOrbCooldowns[orb] = 0;
    }

    public void ActivateOrb()
    {
        if (value <= 0)
        {
            return;
        }

        GameObject orb = _orbs.First(o => _activeOrbCooldowns[o] <= 0);
        if (orb)
        {
            _activeOrbCooldowns[orb] = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        angleOffset += Time.deltaTime;
        float offset = 2 * Mathf.PI / _orbs.Count;

        foreach (GameObject orb in _orbs)
        {
            if (_activeOrbCooldowns[orb] > 0)
            {
                _activeOrbCooldowns[orb] -= Time.deltaTime;
            }

            float angle = angleOffset + _orbs.IndexOf(orb) * offset;
            float radius = _activeOrbCooldowns[orb] > 0 ? 0 : Radius;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector2 targetPosition = Target.transform.position + new Vector3(x, y, 0);
            orb.transform.position = Vector3.Lerp(orb.transform.position, targetPosition, 0.1f);

            SpriteRenderer spriteRenderer = orb.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                spriteRenderer.sortingOrder = orb.transform.position.y > Target.transform.position.y ? -1 : 1;
            }
        }
    }
}
