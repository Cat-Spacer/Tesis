using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum SoundsTypes
{
    WindForest,
    Steps,
    ForestAmbience,
    MusicForest,
    CatDamage,
    CatAttack,
    CatDash,
    CatJump,
    Dash,
    Item,
    Climb,
    FallingDebris,
    WoodAmbientSound,
    FlowerWind,
    Rain,
    CarnivorousPlant,
    PlayerDeath,
    Mushroom,
    VineCrunch,
    MagicCat,
    ClestialDiamond,
    StalacticBreaking,
    MetalFall,
    Button,
    MusicMenu,
    Charging,
    BatteryCollected,
    DoorTuberies,
    Electricity,
    ElectricityFadeOff,
    ElectricityFadeOn,
    ElectricityLoop,
    Generator,
    GeneratorFadeOff,
    GeneratorFadeOn,
    GeneratorLoop,
    MetalDoorTuberies,
    MiniGameWon,
    Owl,
    Spider,
    Platform,
    SpaceShip
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = default;
    public Sound[] sounds = default;
    private float _baseVolume = default;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = default;
    private LookUpTable<string, Sound> _usedSoundsByName = default;

    public Dictionary<string, float> mixerValue = new();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        //InitialSet();
    }

    private void InitialSet()
    {
        //foreach (Sound s in sounds)
        //{
        //    s.source = gameObject.AddComponent<AudioSource>();
        //    s.source.clip = s.clip;
        //    s.source.outputAudioMixerGroup = s.audioMixerGroup;
        //    s.source.volume = s.volume;
        //    s.source.pitch = s.pitch;
        //    s.source.loop = s.loop;
        //}
        _usedSounds = new (SearchSound);
        _usedSoundsByName = new (SearchSound);
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

    private Sound SearchSound(SoundsTypes name)
    {
        return Array.Find(sounds, sound => sound.nameType == name);
    }
    private Sound SearchSound(string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }

    public void Play(SoundsTypes nameType, bool loop = false)
    {
        Sound s = default;

        LinkedList<Sound> repeats = new ();
        foreach (var item in sounds)        
            if (nameType == item.nameType) repeats.Add(item);

        if (repeats.Count > 1)
            s = repeats[UnityEngine.Random.Range(0, repeats.Count)];
        else
            s = _usedSounds.ReturnValue(nameType);

        SoundSet(s);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + nameType + " not found!");
            return;
        }
        s.source.loop = loop;
        s.source.Play();
    }

    public void Play(string name, bool loop = true)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);

        SoundSet(s);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.loop = loop;
        s.source.Play();
    }

    public void Pause(SoundsTypes name)
    {
        Sound s = _usedSounds.ReturnValue(name);

        SoundSet(s);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Pause();
        Destroy(s.source);
    }

    public void Pause(string name)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);

        SoundSet(s);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Pause();
        Destroy(s.source);
    }

    public void PauseAll()
    {
        foreach (var s in sounds) if(s.source) s.source.Pause();
    }

    public IEnumerator FadeOut(AudioSource s, float FadeTime)
    {
        float startVolume = s.volume;

        while (s.volume > 0)
        {
            s.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.Pause();
        s.volume = startVolume;
    }

    public void OnClickSound(string name)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.loop = false;
        s.source.Play();
    }

    public void OnClickSound(SoundsTypes name)
    {
        Sound s = _usedSounds.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.loop = false;
        s.source.Play();
    }
}