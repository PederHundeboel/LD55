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
