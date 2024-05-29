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
        if (!SoundManager.instance)
            Debug.LogWarning($"There is no SoundManager in scene for {gameObject.name}");
        else
        {
            _soundManager = SoundManager.instance;
            mixerName = _audioMixerGroup.name;
            if (!_soundManager.mixerValue.ContainsKey(mixerName))
                _soundManager.mixerValue[mixerName] = slider.value;
            LoadVolumeValues();

        }
    }

    public void SetVolume(float val)
    {
        if (!CheckSoundManger()) return;
        if (_audioMixerGroup == null)
        {
            Debug.LogWarning($"There is no Mixer Group in Audio Mixer Group Variable");
            return;
        }
        _audioMixerGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(val) * 20.0f);
    }

    public void SaveVolumeValues()
    {
        if (!CheckSoundManger()) return;
        _soundManager.mixerValue[mixerName] = slider.value;
    }

    public void LoadVolumeValues()
    {
        slider.value = _soundManager.mixerValue[mixerName];
        _audioMixerGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(slider.value) * 20.0f);
    }

    private bool CheckSoundManger()
    {

        if (!_soundManager)
        {
            Debug.LogWarning($"There is no SoundManager");
        }
        return _soundManager;
    }

    private void OnDisable()
    {
        SaveVolumeValues();
    }
}