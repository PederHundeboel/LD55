using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayOneShotAudioClip(AudioClip clip, Vector3 position)
    {
        GameObject audioSourceGameObject = new GameObject("TempAudio");
        audioSourceGameObject.transform.position = position;

        AudioSource audioSource = audioSourceGameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(audioSourceGameObject, clip.length);
    }
}