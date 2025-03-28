using System;
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

    public void PlayOneShot(AudioClip audioClip, float volume)
    {
        aus.PlayOneShot(audioClip, volume);
    }

    public void PlayOneShot(AudioFile audioFile)
    {
        aus.PlayOneShot(audioFile.clip, audioFile.volume);
    }

    /// <summary>
    /// --Very expensive--, creates a new audiosource, sets it up, plays the audioFile with pitch, and destroys the audioSource.
    /// </summary>
    /// <param name="audioFile"></param>
    /// <param name="pitch"></param>
    public void PlayOneShotWithPitch(AudioFile audioFile, float pitch)
    {
        AudioSource audioSource = new GameObject("Cha-Ching", typeof(AudioSource)).GetComponent<AudioSource>();
        audioSource.pitch = pitch;
        audioSource.volume = audioFile.volume;
        audioSource.clip = audioFile.clip;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioFile.clip.length + 0.1f);
    }
}
