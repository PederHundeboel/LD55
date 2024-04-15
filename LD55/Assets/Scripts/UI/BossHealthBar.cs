using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    public Vector2 offset = new Vector2(0.25f, 0.7f);
    private float segmentOffsetValue = 0.22f;

    private Transform _target;
    
    public OrkHealtBarSegment _segmentPrefab;
    
    [SerializeField] private Sprite _redBar;
    
    
    private List<OrkHealtBarSegment> _segments;

    private void Start()
    {
        _target = transform;
        transform.SetParent(null);
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

    public void SetBar(int count)
    {
        _segments = new List<OrkHealtBarSegment>();
        int index = 0;
        for (int i = 0; i < count; i++)
        {
            var segmentSprite = _redBar;
            OrkHealtBarSegment newSegment = Instantiate(_segmentPrefab, transform);
            newSegment.fullHealthSprite = segmentSprite;
            float offset = segmentOffsetValue*index;
            
            newSegment.transform.localPosition = new Vector3(newSegment.transform.localPosition.x,
                newSegment.transform.localPosition.y + offset, newSegment.transform.localPosition.z);
            newSegment.SetDisplayHealth(true);
            newSegment.SetSortOrder(100+index);
            
            _segments.Add(newSegment);
            index++;
        }
    }


    public void UpdateBar(int value)
    {
        for (int i = 0; i < _segments.Count; i++)
        {
            _segments[i].SetDisplayHealth(i < value);
        }
    }

    public void SetHealth(int i)
    {
        UpdateBar(i);
    }
}
