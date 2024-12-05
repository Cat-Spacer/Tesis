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
    Spray,
    ButtonHover
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = default;
    public Sound[] sounds = default;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = default;
    private LookUpTable<string, Sound> _usedSoundsByName = default;
    [SerializeField] private int _limit = 15;
    [SerializeField] private SoundSpawn _prefab = default;
    private ObjectFactory _objectFactory = default;
    private ObjectPool<ObjectToSpawn> _pool = default;
    private bool _flag = true;
    [SerializeField] private List<SoundSpawn> _soundsList = new();
    [SerializeField] private List<ObjectToSpawn> _poolList = new();

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

    private void Start()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Subscribe(EventType.OnResumeGame, ActiveSounds);
            EventManager.Instance.Subscribe(EventType.OnStartGame, ActiveSounds);
        }
    }

    private void Update()
    {
        _poolList = _pool.GetStock;
    }

    private void InitialSet()
    {
        _usedSounds = new(SearchSound);
        _usedSoundsByName = new(SearchSound);
        _objectFactory = new ObjectFactory(_prefab, transform);
        _pool = new ObjectPool<ObjectToSpawn>(_objectFactory.GetObj, ObjectToSpawn.TurnOnOff, 0);
    }

    public void Play(SoundsTypes nameType, GameObject request = default, bool loop = false)
    {
        Sound s = SearchForRandomSound(nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {nameType} not found!</color>");
            return;
        }

        s = SoundSet(s, request, loop);
        if (s.source) if (s.source.gameObject.activeSelf) s.source.Play();
    }

    public void Play(string name, GameObject request = default, bool loop = true)
    {
        Sound s = SearchForRandomSound(_usedSoundsByName.ReturnValue(name).nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }

        s = SoundSet(s, request, loop);
        if (s.source) if (s.source.gameObject.activeSelf) s.source.Play();
    }

    public void OnClickSound(string name)
    {
        Sound s = SearchForRandomSound(_usedSoundsByName.ReturnValue(name).nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }
        s = SoundSet(s);
        if (s.source) if (s.source.gameObject.activeSelf) s.source.Play();
    }

    public void Pause(SoundsTypes nameType, GameObject request = default)
    {
        Sound s = _usedSounds.ReturnValue(nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {nameType} not found!</color>");
            return;
        }

        s = SoundSet(s, request);
        if (s.source) if (s.source.gameObject.activeSelf) s.source.Pause();
    }

    public void Pause(string name)
    {
        Sound s = _usedSoundsByName.ReturnValue(name);

        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }

        s = SoundSet(s);
        if (s.source) if (s.source.gameObject.activeSelf) s.source.Pause();
    }

    public void PauseAll()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource audioSource in audioSources) audioSource.Pause();
    }

    private Sound SoundSet(Sound s, GameObject request = null, bool loop = false)
    {
        if (s == null) return null;

        SoundSpawn soundObject = null;
        if (request && request != gameObject)
        {
            if (request.GetComponentInChildren<AudioSource>()) if (FoundEqualSound(s.clip, request)) return s;
            soundObject = _pool.GetObject().GetComponent<SoundSpawn>();
        }
        else if (ManagerAudioSourceConfig(s, loop)) return s;

        if (soundObject)
        {
            AddToSoundList(soundObject);
            soundObject.SetFather(request);
            soundObject.AddReferences(_pool);
            //if (GameManager.Instance.pause) soundObject.ReturnToStack(default);               
            return soundObject.SetAudioSource(s, loop);
        }
        else if (s.nameType == SoundsTypes.Music)
        {
            if (_flag)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                _flag = false;
            }
            else
            {
                foreach (AudioSource source in gameObject.GetComponentsInChildren<AudioSource>())
                    if (source.outputAudioMixerGroup == s.audioMixerGroup)
                    {
                        s.source = source;
                        break;
                    }
                if (!s.source) s.source = gameObject.GetComponent<AudioSource>();
            }
        }
        else s.source = gameObject.AddComponent<AudioSource>();

        if (s.source == null)
        {
            Debug.LogWarning($"<color=orange>Source not found!</color>");
            return s;
        }
        //if (soundObject)
        //{
        //    soundObject.AddReferences(_pool);
        //}
        //else
        //{
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = loop;
        //}

        return s;
    }

    private bool ManagerAudioSourceConfig(Sound s, bool loop)
    {
        AudioSource[] allSourcess = GetComponents<AudioSource>();
        int count = 0;
        foreach (AudioSource source in allSourcess)
        {
            if (source.clip == s.clip) return true;
            if (source.outputAudioMixerGroup == s.audioMixerGroup)
            {
                count++;
                if (count == _limit)
                {
                    s.source = source;
                    source.clip = s.clip;
                    source.outputAudioMixerGroup = s.audioMixerGroup;
                    source.volume = s.volume;
                    source.pitch = s.pitch;
                    source.loop = loop;
                    return true;
                }
            }
        }
        return false;
    }

    private Sound SearchSound(SoundsTypes name)
    {
        return Array.Find(sounds, sound => sound.nameType == name);
    }
    private Sound SearchSound(string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }

    private bool FoundEqualSound(AudioClip clip, GameObject target)
    {
        AudioSource[] audioSources = target.GetComponentsInChildren<AudioSource>();
        return Array.Find(audioSources, source => source.clip == clip);
    }

    public Sound SearchForRandomSound(SoundsTypes nameType)
    {
        LinkedList<Sound> repeats = new();
        foreach (var item in sounds)
            if (nameType == item.nameType) repeats.Add(item);

        Sound s = default;
        if (repeats.Count > 1)
            s = repeats[UnityEngine.Random.Range(0, repeats.Count)];
        else
            s = _usedSounds.ReturnValue(nameType);
        return s;
    }

    public void RemoveFromSoundList(SoundSpawn sound)
    {
        Debug.Log($"{sound} was removed");

        if (_soundsList != null) if (_soundsList.Count > 0) if (_soundsList.Contains(sound)) _soundsList.Remove(sound);
    }

    public void AddToSoundList(SoundSpawn sound)
    {
        if (!_soundsList.Contains(sound)) _soundsList.Add(sound);
    }

    private void ActiveSounds(object[] obj)
    {
        foreach (var sound in _soundsList) sound.Reset();
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
    }
}