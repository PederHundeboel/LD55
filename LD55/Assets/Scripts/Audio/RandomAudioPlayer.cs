using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClipLibrary[] audioClipLibraries;

    [System.Serializable]
    public struct AudioClipLibrary
    {
        public string libraryName;
        public AudioClip[] clips;
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string soundType)
    {
        foreach (AudioClipLibrary acl in audioClipLibraries)
        {
            if (acl.libraryName == soundType)
            {
                AudioClip clip = GetRandomClip(acl.clips);
                audioSource.PlayOneShot(clip);
            }
        }
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}

