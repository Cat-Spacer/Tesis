using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    //[SerializeField] private string soundPlayName = "Sound";
    [SerializeField] private SoundManager.Types[] _soundPlayName;
    [SerializeField] private bool _loop = false;
    [SerializeField] private bool _pauseAll = false;

    private void Start()
    {
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
            return;
        foreach (var sound in _soundPlayName)
            soundManager.Play(sound, _loop);
        if (_pauseAll)
            soundManager.PauseAll();
    }
}
