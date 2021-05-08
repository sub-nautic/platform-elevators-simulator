using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;

    AudioSource audioSource;
    bool isPlaying;
    bool isDifPlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDefaultAudio()
    {
        if (!isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    public bool ResetDiffAudio() { return isDifPlaying = false; }
    public void PlayDiffrentAudio(int index)
    {
        if (!isDifPlaying)
        {
            audioSource.PlayOneShot(clips[index]);
            isDifPlaying = true;
        }
    }

    public void StopAudio()
    {
        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}
