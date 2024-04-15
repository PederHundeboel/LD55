using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrkBoss : MonoBehaviour
{
    public GameObject peonPrefab;
    public List<GameObject> spawnPoints;
    //this is omegascuffed but it's fine for now
    public List<SpellResources.SpellType> _healthBar;
    private GameObject _player;
    private bool _isSummoning = false;

    private void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
        for (int i = 0; i < 10; i++)
        {
            _healthBar.Add((SpellResources.SpellType)Random.Range(0, 3));
        }
        foreach (var spellType in _healthBar)
        {
            Debug.Log(spellType);
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

    public void HitWithSpell(List<SpellResources.SpellType> dictionary)
    {
        //this is untested for now (note for mr unbreakable)
        // int count = Math.Min(3, _healthBar.Count);
        // for (int i = 0; i < count; i++)
        // {
        //     if (dictionary.ContainsKey(_healthBar[i]))
        //     {
        //         dictionary[_healthBar[i]]--;
        //         if (dictionary[_healthBar[i]] == 0)
        //         {
        //             dictionary.Remove(_healthBar[i]);
        //         }
        //         _healthBar.RemoveAt(i);
        //         i--;
        //         count--; 
        //     }
        //     else
        //     {
        //         _healthBar.Add((SpellResources.SpellType)Random.Range(0, 3));
        //     }
        // }
        //for debug purposes, log the health bar to the console
        foreach (var spellType in _healthBar)
        {
            Debug.Log(spellType);
        }
        
        if (_healthBar.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
