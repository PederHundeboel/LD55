using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip defaultMusic;
    public AudioClip triggeredMusic;
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.clip = defaultMusic;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Called music trigger");
        if (other.CompareTag("Player"))
        {
            audioSource.clip = triggeredMusic;
            audioSource.Play();
        }
    }
}
