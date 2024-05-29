using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    void Start()
    {
        if(SoundManager.instance) sounds = SoundManager.instance.sounds;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    public void StopAll() { SoundManager.instance.PauseAll(); }
}