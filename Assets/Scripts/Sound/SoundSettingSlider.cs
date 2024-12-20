using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class SoundSettingSlider : MonoBehaviour
{
    private Slider Slider => GetComponent<Slider>();

    public string mixerName = null;
    [SerializeField] private AudioMixerGroup _audioMixerGroup = null;
    private SoundManager _soundManager = null;

    private void Awake()
    {
        if (!SoundManager.instance)
            Debug.LogWarning($"There is no SoundManager in scene for {gameObject.name}");
        else
        {
            _soundManager = SoundManager.instance;
            mixerName = _audioMixerGroup.name;
            _soundManager.MixerValue.TryAdd(mixerName, 1);
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
        _soundManager.SetMixerVolume(mixerName, Slider.value);
    }

    private void LoadVolumeValues()
    {
        Slider.value = _soundManager.MixerValue[mixerName];
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