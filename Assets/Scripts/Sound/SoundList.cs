using System;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = null;
    private LookUpTable<string, Sound> _usedSoundsByName = null;

    private void Start()
    {
        if (SoundManager.instance) sounds = SoundManager.instance.sounds;
        _usedSounds = new LookUpTable<SoundsTypes, Sound>(SearchSound);
        _usedSoundsByName = new LookUpTable<string, Sound>(SearchSound);
    }

    private Sound SearchSound(SoundsTypes nameType)
    {
        return Array.Find(sounds, sound => sound.nameType == nameType);
    }
    private Sound SearchSound(string soundName)
    {
        return Array.Find(sounds, sound => sound.name == soundName);
    }

    public void Play(string soundName)
    {
        Sound s = _usedSoundsByName.ReturnValue(soundName);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {soundName} not found!</color>");
            return;
        }
        if (SoundManager.instance) SoundManager.instance.Play(soundName, null, true);
        else
        {
            SoundSet(s);
            s.source.loop = true;
            s.source.Play();
        }
    }

    public void PlayClip(string soundName)
    {
        Sound s = _usedSoundsByName.ReturnValue(soundName);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {soundName} not found!</color>");
            return;
        }

        if (SoundManager.instance) SoundManager.instance.Play(s.nameType);
        else
        {
            Debug.Log($"<color=orange>No SoundManage</color>");
            SoundSet(s);
            s.source.loop = false;
            s.source.Play();
        }
    }

    public void Play(SoundsTypes nameType, bool loop = false)
    {
        Sound s = _usedSounds.ReturnValue(nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {nameType} not found!</color>");
            return;
        }
        if (SoundManager.instance) SoundManager.instance.Play(nameType, null, loop);
        else
        {
            SoundSet(s);
            s.source.loop = loop;
            s.source.Play();
        }
    }

    private void SoundSet(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = s.audioMixerGroup;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }
}