using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeAudioClip : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.spatialize = true;
        _audioSource.spatialBlend = 1;
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
