using System;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = default;
    private LookUpTable<string, Sound> _usedSoundsByName = default;

    void Start()
    {
        if (SoundManager.instance) sounds = SoundManager.instance.sounds;
        _usedSounds = new LookUpTable<SoundsTypes, Sound>(SearchSound);
        _usedSoundsByName = new LookUpTable<string, Sound>(SearchSound);
    }

    private Sound SearchSound(SoundsTypes name)
    {
        return Array.Find(sounds, sound => sound.nameType == name);
    }
    private Sound SearchSound(string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }

    public void Play(string name)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }
        if (SoundManager.instance) SoundManager.instance.Play(name);
        else
        {
            SoundSet(s);
            s.source.loop = true;
            s.source.Play();
        }
    }

    public void PlayClip(string name)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
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

    public void Play(SoundsTypes name, bool loop = false)
    {
        Sound s = _usedSounds.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }
        if (SoundManager.instance) SoundManager.instance.Play(name, null, loop);
        else
        {
            SoundSet(s);
            s.source.loop = loop;
            s.source.Play();
        }
    }

    public void PlayOneShot(SoundsTypes name)
    {
        Sound s = _usedSounds.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }
        if (SoundManager.instance) SoundManager.instance.Play(name);
        else
        {
            SoundSet(s);
            s.source.loop = false;
            s.source.Play();
        }
    }

    public void StopAll() { SoundManager.instance.PauseAll(); }

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