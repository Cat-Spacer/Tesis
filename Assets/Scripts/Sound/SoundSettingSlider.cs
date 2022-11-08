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
    private SoundManager _soundManager;

    private void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        if (_soundManager == null)
            Debug.LogError($"There is no SoundManager in scene for {gameObject.name}");
        mixerName = _audioMixerGroup.name;
        if (!_soundManager.mixerValue.ContainsKey(mixerName))
            _soundManager.mixerValue[mixerName] = slider.value;
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
        //PlayerPrefs.SetFloat(mixerName, slider.value);
        _soundManager.mixerValue[mixerName] = slider.value;
    }

    public void LoadVolumeValues()
    {
        //slider.value = PlayerPrefs.GetFloat(mixerName);
        slider.value = _soundManager.mixerValue[mixerName];
        _audioMixerGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(slider.value) * 20.0f);
    }

    private void OnDisable()
    {
        SaveVolumeValues();
    }
}
