using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WalkAudioController : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    AudioSource audioSource;
    Vector3 pos, velocity;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pos = transform.position;

    }

    void FixedUpdate()
    {
        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;

        if (Mathf.Abs(velocity.magnitude) <= 0 || audioSource.isPlaying) return;
        var clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.clip = clip;
        audioSource.Play();
    }
}
