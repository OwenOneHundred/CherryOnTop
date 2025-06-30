using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager sfxmanager;
    private AudioSource aus;
    List<AudioClipAndTime> audioClipsPlayedThisFrame = new();
    [SerializeField] float timeBetweenSounds = 1f;
    [SerializeField] UnityEngine.Audio.AudioMixerGroup sfxMixerGroup;
    void Awake()
    {
        if (sfxmanager == null || sfxmanager == this)
        {
            sfxmanager = this;
            DontDestroyOnLoad(transform.root);
        }
        else 
        {
            Destroy(transform.root.gameObject);
        }
    }

    void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip audioClip, float volume)
    {
        if (CheckIfBeenPlayedThisFrame(audioClip)) { return; }
        aus.PlayOneShot(audioClip, volume);
        audioClipsPlayedThisFrame.Add(new AudioClipAndTime(audioClip));
    }

    public void PlayOneShot(AudioFile audioFile)
    {
        if (CheckIfBeenPlayedThisFrame(audioFile.clip)) { return; }
        aus.PlayOneShot(audioFile.clip, audioFile.volume);
        audioClipsPlayedThisFrame.Add(new AudioClipAndTime(audioFile.clip));
    }

    public bool TryPlayOneShot(AudioFile audioFile)
    {
        if (audioFile == null || audioFile.clip == null) { return false; }
        else
        {
            PlayOneShot(audioFile);
            return true;
        }
    }

    private bool CheckIfBeenPlayedThisFrame(AudioClip audioClip)
    {
        return !(audioClipsPlayedThisFrame.FirstOrDefault(x => x.clip == audioClip) == null);
    }

    /// <summary>
    /// --Very expensive--, creates a new audiosource, sets it up, plays the audioFile with pitch, and destroys the audioSource.
    /// </summary>
    /// <param name="audioFile"></param>
    /// <param name="pitch"></param>
    public void PlayOneShotWithPitch(AudioFile audioFile, float pitch)
    {
        if (CheckIfBeenPlayedThisFrame(audioFile.clip)) { return; }
        AudioSource audioSource = new GameObject("Cha-Ching", typeof(AudioSource)).GetComponent<AudioSource>();
        audioSource.pitch = pitch;
        audioSource.volume = audioFile.volume;
        audioSource.clip = audioFile.clip;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioFile.clip.length * 2.05f);
        audioClipsPlayedThisFrame.Add(new AudioClipAndTime(audioFile.clip));
    }

    void Update()
    {
        foreach (AudioClipAndTime audioClipAndTime in audioClipsPlayedThisFrame)
        {
            audioClipAndTime.timeSinceLast += Time.deltaTime;
        }
        audioClipsPlayedThisFrame.RemoveAll(x => x.timeSinceLast > timeBetweenSounds);
    }

    class AudioClipAndTime
    {
        public AudioClipAndTime(AudioClip audioClip)
        {
            clip = audioClip;
            timeSinceLast = 0;
        }
        public AudioClip clip;
        public float timeSinceLast;
    }
}
