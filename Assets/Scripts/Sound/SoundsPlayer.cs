using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private SoundsTypes[] _soundPlayName;
    [SerializeField] private bool _loop = false, _pauseAll = false, _instance = false;

    private void Start()
    {
        if (SoundManager.instance == null) return;

        if (_pauseAll) SoundManager.instance.PauseAll();

        foreach (var sound in _soundPlayName) SoundManager.instance.Play(sound, _instance ? gameObject : null, _loop);
    }
}