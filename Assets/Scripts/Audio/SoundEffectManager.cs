using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager sfxmanager;
    private AudioSource aus;
    void Awake()
    {
        if (sfxmanager == null || sfxmanager == this)
        {
            sfxmanager = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    public void TryPlayOneShot(AudioFile audioFile)
    {
        if (audioFile == null) { return; }
        if (audioFile.volume == 0) { return; }

        aus.PlayOneShot(audioFile.clip, audioFile.volume);
    }

    public void TryPlayOneShot(AudioClip audioClip, float volume)
    {
        if (audioClip == null) { return; }
        if (volume == 0) { return; }

        aus.PlayOneShot(audioClip, volume);
    }

    public void PlayOneShot(AudioFile audioFile)
    {
        aus.PlayOneShot(audioFile.clip, audioFile.volume);
    }
}
