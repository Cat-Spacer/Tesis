using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name, bool loop)
    {
        if (sounds == null) return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.loop = loop;
        s.source.Play();
    }

    public void PlayLoop(string name)
    {
        if (sounds == null) return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.loop = true;
        s.source.Play();
    }

    public void PlayButton(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public Sound GetSound(string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }
}
