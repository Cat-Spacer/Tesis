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
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Play(SoundsTypes name, bool loop = false)
    {
        Sound s = _usedSounds.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.loop = loop;
        s.source.Play();
    }
    public void StopAll() { SoundManager.instance.PauseAll(); }
}