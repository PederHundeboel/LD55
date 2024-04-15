using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrkBoss : MonoBehaviour
{
    public GameObject peonPrefab;
    public List<GameObject> spawnPoints;
    public Container health;
    private GameObject _player;
    private bool _isSummoning = false;

    List<BossShield> _shields = new List<BossShield>();

    public List<Transform> _shieldPositions;
    
    public BossShieldIndicator shieldIndicatorPrefab;

    public List<BossShieldIndicator> shieldIndicators;
    private void Start()
    {
        //add 3 shields
        for (int i = 0; i < 3; i++)
        {
            _shields.Add(new BossShield());
        }
        _player = FindObjectOfType<Player>().gameObject;
        
        
        health.SetMax(3);
        health.Add(3);
        
        SetShieldBars();
    }

    private void SetShieldBars()
    {
        for (int i = 0; i < _shields.Count; i++)
        {
            var shieldIndicator = Instantiate(shieldIndicatorPrefab, Vector3.zero, Quaternion.identity, _shieldPositions[i]);
            shieldIndicator.SetBar(_shields[i].sequence);
            shieldIndicators.Add(shieldIndicator);
        }
    }

    private void Update()
    {
        if (!_isSummoning && Vector3.Distance(transform.position, _player.transform.position) < 20)
        {
            _isSummoning = true;
            InvokeRepeating(nameof(SpawnPeon), 0, 10);
        }
        if (_isSummoning && Vector3.Distance(transform.position, _player.transform.position) > 20)
        {
            _isSummoning = false;
            CancelInvoke();
        }
    }

    public void SpawnPeon()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPosition = spawnPoints[randomIndex].transform.position;

        var peon = Instantiate(peonPrefab, spawnPosition, Quaternion.identity);
        peon.GetComponent<OrkController>().SetTarget(_player.transform);
    }

    public void HitWithSpell(List<SpellResources.SpellType> hit)
    {
        var shield = _shields.Find(s => !s.isDestroyed && s.AttemptAttack(hit));
        if (shield != null)
        {
            var shieldIndex = _shields.IndexOf(shield);
            var shieldIndicator = shieldIndicators[shieldIndex];
            shieldIndicator.EmptyBar();
            health.Subtract(1);
            shield.isDestroyed = true;
        }

        if (health.GetValue() == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
    
    public class BossShield
    {
        public bool isDestroyed = false;
        public List<SpellResources.SpellType> sequence = new List<SpellResources.SpellType>();

        public BossShield()
        {
            for (int i = 0; i < 3; i++)
            {
                sequence.Add((SpellResources.SpellType)Random.Range(0, 3));
            }
        }
        public bool AttemptAttack(List<SpellResources.SpellType> hit)
        {
            //idk whats going on elp
            List<SpellResources.SpellType> copything = new List<SpellResources.SpellType>();
            copything.AddRange(hit);
            var matched = 0;
            foreach (var s in sequence)
            {
                //find match in hit, remove it. add 1 to matched
                if (copything.Contains(s))
                {
                    copything.Remove(s);
                    matched++;
                }
            }
            return matched == 3;
        }
    }
}
