using UnityEngine;

public class ButtonSoundScript : MonoBehaviour
{
    [SerializeField] AudioFile audioFile;
    public void PlaySound()
    {
        SoundEffectManager.sfxmanager.PlayOneShot(audioFile);
    }
}
