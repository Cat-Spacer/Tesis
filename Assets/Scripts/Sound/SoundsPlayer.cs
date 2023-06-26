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
        if (SoundManager.instance == null)
            return;

        foreach (var sound in _soundPlayName)
            SoundManager.instance.Play(sound, _loop);

        if (_pauseAll)
            SoundManager.instance.PauseAll();
    }
}