using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class SoundSettingSlider : MonoBehaviour
{
    Slider slider
    {
        get { return GetComponent<Slider>(); }
    }
    public string mixerName;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    private void Awake()
    {
        LoadVolumeValues();
    }

    public void SetVolume(float val)
    {
        if (_audioMixerGroup == null)
        {
            Debug.LogError($"There is no Mixer Group in Audio Mixer Group Variable");
            return;
        }
        _audioMixerGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(val) * 20.0f);
    }

    public void SaveVolumeValues()
    {
        PlayerPrefs.SetFloat(mixerName, slider.value);
    }

    public void LoadVolumeValues()
    {
        slider.value = PlayerPrefs.GetFloat(mixerName, slider.value);
        _audioMixerGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(slider.value) * 20.0f);
    }

    private void OnDisable()
    {
        SaveVolumeValues();
    }
}
