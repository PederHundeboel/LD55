using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ContainerBar : MonoBehaviour
{
    [SerializeField]
    private Container _container;
    [SerializeField]
    private ContainerBarSegment _segmentPrefab;
    [SerializeField]
    private List<ContainerBarSegment> segment;
    
    private int _chunksPerUnit = 0;

    private void Awake()
    {
        SetBar();
        UpdateBar();
    }

    public void SetBar()
    {
        for (int i = 0; i < segment.Count; i++)
        {
            Destroy(segment[i].gameObject);
        }

        segment.Clear();
        for (int i = 0; i < _container.GetMax(); i++)
        {
            ContainerBarSegment newSegment = Instantiate(_segmentPrefab, transform);
            //offset the segments
            float offset = (float)(i * (newSegment.GetComponent<RectTransform>().rect.height * 0.5));
            newSegment.transform.localPosition = new Vector3(newSegment.transform.localPosition.x,
                newSegment.transform.localPosition.y + offset, newSegment.transform.localPosition.z);
            segment.Add(newSegment);
        }
        _chunksPerUnit = _container.GetMax() / segment.Count;
    }

    public void UpdateBar()
    {
        int value = _container.GetValue();

        for (int i = 0; i < segment.Count; i++)
        {
            segment[i].SetDisplayHealth((i + 1) * _chunksPerUnit <= value);
        }
    }
}