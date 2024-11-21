using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum SoundsTypes
{
    Steps,
    CatAttack,
    CatDamage,
    CatDash,
    CatGrab,
    CatJump,
    Dash,
    Item,
    Climb,
    FallingDebris,
    Death,
    StalacticBreaking,
    MetalFall,
    Button,
    Click,
    Music,
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
    SpaceShip,
    HamsterSteps,
    HamsterAttack,
    HamsterJump,
    HamsterDamage,
    HamsterOnTubes,
    Block,
    CircularSaw,
    Laser,
    BeamShoot,
    DumpingTrash,
    Push,
    Checpoint,
    Collect,
    Interact,
    Drop,
    Lever,
    PressureLever,
    Spray
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = default;
    public Sound[] sounds = default;
    private float _baseVolume = default;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = default;
    private LookUpTable<string, Sound> _usedSoundsByName = default;
    private LookUpTable<GameObject, SoundSpawn> _usedSoundsByRequest = default;
    [SerializeField] private int _limit = 15;
    [SerializeField] private SoundSpawn _prefab = default;
    private ObjectFactory _objectFactory = default;
    private ObjectPool<ObjectToSpawn> _pool = default;
    private bool _found = false;

    public Dictionary<string, float> mixerValue = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitialSet();
    }

    private void InitialSet()
    {
        _usedSounds = new(SearchSound);
        _usedSoundsByName = new(SearchSound);
        _usedSoundsByRequest = new(SearchSoundSpawn);
        _objectFactory = new ObjectFactory(_prefab, transform);
        _pool = new ObjectPool<ObjectToSpawn>(_objectFactory.GetObj, ObjectToSpawn.TurnOnOff, 0);
    }

    private void SoundSet(Sound s, GameObject request = default)
    {
        if (s == null) return;
        AudioSource[] allSourcess = GetComponents<AudioSource>();
        int count = 0;
        foreach (AudioSource source in allSourcess)
        {
            if (source.clip == s.clip) return;
            if (source.outputAudioMixerGroup == s.audioMixerGroup)
            {
                count++;
                if (count == _limit)
                {
                    source.clip = s.clip;
                    source.outputAudioMixerGroup = s.audioMixerGroup;
                    source.volume = s.volume;
                    source.pitch = s.pitch;
                    source.loop = s.loop;
                    return;
                }
            }
        }
        SoundSpawn soundObject = null;
        if (request && request != gameObject) soundObject = _usedSoundsByRequest.ReturnValue(request);
        if (soundObject)
        {
            if (!_found)
            {
                soundObject.SetFather(request);
                s.source = soundObject.gameObject.AddComponent<AudioSource>();
            }
        }
        else s.source = gameObject.AddComponent<AudioSource>();
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
    private SoundSpawn SearchSoundSpawn(GameObject requester)
    {
        if (!requester) return null;
        SoundSpawn spawn = Array.Find(_pool.GetStock.ToArray(), stock => stock.GetComponent<SoundSpawn>().Father == requester).GetComponent<SoundSpawn>();
        _found = spawn ? true :  false;
        return spawn ? spawn : _pool.GetObject().GetComponent<SoundSpawn>();
    }

    public void Play(SoundsTypes nameType, bool loop = false, GameObject request = default)
    {
        Sound s = default;

        LinkedList<Sound> repeats = new();
        foreach (var item in sounds)
            if (nameType == item.nameType) repeats.Add(item);

        if (repeats.Count > 1)
            s = repeats[UnityEngine.Random.Range(0, repeats.Count)];
        else
            s = _usedSounds.ReturnValue(nameType);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + nameType + " not found!");
            return;
        }
        SoundSet(s, request);
        s.source.loop = loop;
        s.source.Play();
        if (!loop) StartCoroutine(AutoStop(s));
    }

    public void Play(string name, bool loop = true, GameObject request = default)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);
        LinkedList<Sound> repeats = new();
        foreach (var item in sounds)
            if (name == item.name) repeats.Add(item);

        if (repeats.Count > 1)
            s = repeats[UnityEngine.Random.Range(0, repeats.Count)];
        else
            s = _usedSoundsByName.ReturnValue(name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        SoundSet(s, request);
        s.source.loop = loop;
        s.source.Play();
        if (!loop) StartCoroutine(AutoStop(s));
    }

    public void Pause(SoundsTypes name)
    {
        Sound s = _usedSounds.ReturnValue(name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        SoundSet(s);
        s.source.Pause();
        //Destroy(s.source);
    }

    public void Pause(string name)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        SoundSet(s);
        s.source.Pause();
        //Destroy(s.source);
    }

    public void PauseAll()
    {
        foreach (var s in sounds) if (s.source) s.source.Pause();
    }

    public IEnumerator AutoStop(Sound s)
    {
        yield return new WaitForSeconds(s.clip.length);
        Pause(s.name);
    }

    public IEnumerator FadeOut(AudioSource source, float FadeTime)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        source.volume = startVolume;
        source.Pause();
        //Destroy(source);
    }

    public void OnClickSound(string name)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        SoundSet(s);
        s.source.loop = false;
        s.source.Play();
        StartCoroutine(AutoStop(s));
    }

    public void OnClickSound(SoundsTypes name)
    {
        Sound s = _usedSounds.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        SoundSet(s);
        s.source.loop = false;
        s.source.Play();
        StartCoroutine(AutoStop(s));
    }
}