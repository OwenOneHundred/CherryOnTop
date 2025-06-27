using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] List<VolumeSlider> volumeSliders;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject settingsMenuGameObject;
    [SerializeField] Vector2 volumeRange;

    void Awake()
    {
        OnStart();
    }

    public void OnStart()
    {
        LoadAllSliders();
        UpdateVolumes();
    }

    public void Open()
    {
        settingsMenuGameObject.SetActive(true);
    }

    public void Close()
    {
        SaveAllSliders();
        settingsMenuGameObject.SetActive(false);
    }

    public void Update()
    {
        UpdateVolumes();
    }

    public void QuitGameIfNotUnityEditor()
    {
        SaveAllSliders();
        Application.Quit();
    }

    public void OnRestartPressed()
    {
        SaveAllSliders();
    }

    public void GotoMainMenu()
    {
        SaveAllSliders();
        if (LevelManager.Instance != null && LevelManager.Instance.roundManager.roundState != RoundManager.RoundState.cherries)
        {
            LevelManager.Instance.SaveLevel();
        }
        Time.timeScale = 1;
        TransitionManager.transitionManager.LoadScene("MenuScene");
    }

    private void LoadAllSliders()
    {
        foreach (VolumeSlider volumeSlider in volumeSliders)
        {
            LoadSlider(volumeSlider);
        }
    }

    private void SaveAllSliders()
    {
        foreach (VolumeSlider volumeSlider in volumeSliders)
        {
            SaveSlider(volumeSlider);
        }
    }

    private void LoadSlider(VolumeSlider volumeSlider)
    {
        if (PlayerPrefs.HasKey(volumeSlider.name))
        {
            Debug.Log(PlayerPrefs.GetFloat(volumeSlider.name));
            volumeSlider.slider.value = PlayerPrefs.GetFloat(volumeSlider.name);
        }
        else
        {
            volumeSlider.slider.value = volumeSlider.defaultVolume;
        }
    }

    private void SaveSlider(VolumeSlider volumeSlider)
    {
        PlayerPrefs.SetFloat(volumeSlider.name, volumeSlider.slider.value);
        PlayerPrefs.Save();
    }

    private void UpdateVolumes()
    {
        foreach (VolumeSlider volumeSlider in volumeSliders)
        {
            audioMixer.SetFloat(volumeSlider.parameterName, ValueToVolume(volumeSlider.slider.value));
        }
    }

    private float ValueToVolume(float value)
    {
        return value == 0 ? -80 : Mathf.Lerp(volumeRange.x, volumeRange.y, value);
    }


    [System.Serializable]
    private class VolumeSlider
    {
        public string name;
        public Slider slider;
        public string parameterName;
        public float defaultVolume = 0;
    }
}
