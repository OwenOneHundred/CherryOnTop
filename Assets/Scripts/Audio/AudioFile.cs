using UnityEngine;

[System.Serializable]
public class AudioFile
{
    [Range(0, 1)]
    public float volume = 0.5f;
    public AudioClip clip;
}
