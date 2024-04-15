using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldIndicator : MonoBehaviour
{
    public Vector2 offset = new Vector2(0.25f, 0.7f);
    private float segmentOffsetValue = 0.22f;

    [SerializeField] private Transform _target;
    
    public OrkHealtBarSegment _segmentPrefab;
    
    [SerializeField] private Sprite _greenBar;
    [SerializeField] private Sprite _blueBar;
    [SerializeField] private Sprite _redBar;
    
    
    private List<OrkHealtBarSegment> _segments;

    private void Start()
    {
        _target = transform.parent;
        transform.SetParent(null);
        //get the target's health bar
        //set the health bar scale to .5
        transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    private void Update()
    {
        if (_target == null)
        {
            //delayed destroy
            Destroy(gameObject, 0.1f);
        }
        else
        {
            transform.position = _target.transform.position + new Vector3(offset.x, offset.y, 0);
        }
    }

    public void SetBar(List<SpellResources.SpellType> healthBar)
    {
        _segments = new List<OrkHealtBarSegment>();
        int index = 0;
        foreach (var chunk in healthBar)
        {
            var segmentSprite = GetSegment(chunk);
            OrkHealtBarSegment newSegment = Instantiate(_segmentPrefab, transform);
            newSegment.fullHealthSprite = segmentSprite;
            float offset = segmentOffsetValue*index;
            
            newSegment.transform.localPosition = new Vector3(newSegment.transform.localPosition.x,
                newSegment.transform.localPosition.y + offset, newSegment.transform.localPosition.z);
            newSegment.SetDisplayHealth(chunk != SpellResources.SpellType.None);
            newSegment.SetSortOrder(100+index);
            
            _segments.Add(newSegment);
            index++;
        }
    }

    private Sprite GetSegment(SpellResources.SpellType chunk)
    {
        switch (chunk)
        {
            case SpellResources.SpellType.Utility:
                return _greenBar;
            case SpellResources.SpellType.Defensive:
                return _blueBar;
            case SpellResources.SpellType.Offensive:
                return _redBar;
            default:
                return _greenBar;
        }
    }

    public void EmptyBar()
    {
        foreach (var segment in _segments)
        {
            segment.SetDisplayHealth(false);
        }
    }
    
    public void UpdateBar(List<SpellResources.SpellType> healthBar)
    {
        //sort any "none" elements of health bar to the end
        healthBar.Sort((x, y) => x == SpellResources.SpellType.None ? 1 : 0);
        for (int i = 0; i < healthBar.Count; i++)
        {
            _segments[i].fullHealthSprite = GetSegment(healthBar[i]);
            _segments[i].SetDisplayHealth(healthBar[i] != SpellResources.SpellType.None);
            _segments[i].SetSortOrder(100+i);
        }
    }
}
