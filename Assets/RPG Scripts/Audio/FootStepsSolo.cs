using UnityEngine;

public class FootStepsSolo : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioSource audioSource = null;

    void Awake()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}