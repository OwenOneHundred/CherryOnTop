using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    readonly float lowpassThreshold = 15000;

    /// <summary>
    /// Sets lowpass. Value of 0 turns off lowpass, value of 1 allows no sound through.
    /// Lowpass scales weird so a lowpass of 0.3 cuts off about half of sound.
    /// </summary>
    /// <param name="value0to1"></param>
    public void SetLowpass(float value0to1)
    {
        float lowpassNumber = Mathf.Lerp(10, lowpassThreshold, 1 - value0to1);
        if (value0to1 <= 0)
        {
            audioMixer.SetFloat("MusicLowpass", 22000);
        }
        else
        {
            audioMixer.SetFloat("MusicLowpass", lowpassNumber);
        }
    }
}
