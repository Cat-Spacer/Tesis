using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private string soundPlayName = "Sound";
    [SerializeField] private bool loop = false;

    private void Start()
    {
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        soundManager.Play(soundPlayName, loop);
    }
}
