using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    //[SerializeField] private string soundPlayName = "Sound";
    [SerializeField] private SoundsTypes[] _soundPlayName;
    [SerializeField] private bool _loop = false;
    [SerializeField] private bool _pauseAll = false;

    private void Start()
    {
        if (SoundManager.instance == null) return;

        if (_pauseAll) SoundManager.instance.PauseAll();

        foreach (var sound in _soundPlayName) SoundManager.instance.Play(sound, null, _loop);
    }
}