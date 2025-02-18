using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private double loopEndTime;
    private int loopStartSamples;
    private int loopEndSamples;
    private int loopLengthSamples;
    [System.NonSerialized] public bool playing = true;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] Song defaultSong;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        RecalculateSamples(defaultSong);

        audioSource.clip = defaultSong.clip;
        if (playOnAwake)
        {
            audioSource.Play();
        }
    }

    private void RecalculateSamples(Song song)
    {
        loopStartSamples = (int)(song.loopStartTime * song.clip.frequency);
        loopEndTime = song.clip.length - song.timeCutOffFromEnd;
        loopEndSamples = (int)(loopEndTime * song.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;
    }

    private void Update()
    {
        if (audioSource.timeSamples >= loopEndSamples)
        {
            audioSource.timeSamples -= loopLengthSamples;
            audioSource.Play();
        }
    }

    public void Play() 
    {
        playing = false;
        audioSource.Play();
    }

    public void Pause()
    {
        playing = true;
        audioSource.Stop();
    }

    public void ChangeSong(Song song)
    {
        StartCoroutine(ChangeSongCoroutine(song));
    }

    private IEnumerator ChangeSongCoroutine(Song song)
    {
        while (audioSource.volume > 0.001f)
        {
            audioSource.volume = Mathf.Clamp(audioSource.volume - Time.deltaTime, 0, 10);
            yield return null;
        }
        audioSource.Stop();

        audioSource.clip = song.clip;
        audioSource.volume = song.volume;

        RecalculateSamples(song);
        audioSource.Play();
    }
}
