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
    private float _baseVolume = default;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = default;
    private LookUpTable<string, Sound> _usedSoundsByName = default;
    private LookUpTable<GameObject, SoundSpawn> _searchRequest = default;
    [SerializeField] private int _limit = 15;
    [SerializeField] private SoundSpawn _prefab = default;
    private ObjectFactory _objectFactory = default;
    private ObjectPool<ObjectToSpawn> _pool = default;
    private bool _found = false, _pause = true, _flag = true;

    public bool pause { get { return _pause; } }

    public Dictionary<string, float> mixerValue = new();
    [SerializeField] private List<AudioSource> _externalGOList = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        _pause = true;
        DontDestroyOnLoad(gameObject);
        InitialSet();
    }

    private void Start()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Subscribe(EventType.OnStartGame, ResumeAllButNoMusic);
            EventManager.Instance.Subscribe(EventType.OnResumeGame, ResumeAllButNoMusic);
            EventManager.Instance.Subscribe(EventType.OnLoseGame, PauseAllButNoMusic);
            EventManager.Instance.Subscribe(EventType.OnPauseGame, PauseAllButNoMusic);
            EventManager.Instance.Subscribe(EventType.OnFinishGame, PauseAllButNoMusic);
        }
    }

    private void InitialSet()
    {
        _usedSounds = new(SearchSound);
        _usedSoundsByName = new(SearchSound);
        _searchRequest = new(SearchSoundSpawn);
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

        s = SoundSet(s, request);
        if (s.source == null)
        {
            Debug.LogWarning($"<color=orange>Source: {nameType} not found!</color>");
            return;
        }
        s.source.loop = loop;
        s.source.Play();
    }

    public void Play(string name, GameObject request = default, bool loop = true)
    {
        Sound s = SearchForRandomSound(_usedSoundsByName.ReturnValue(name).nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }

        s = SoundSet(s, request);
        s.source.loop = loop;
        s.source.Play();
    }

    public void PlayAll(SoundsTypes exception = SoundsTypes.Music)
    {
        //Debug.Log($"<color=orange>Sound: {ToString()} PlayAll function enter !</color>");
        foreach (Sound s in sounds) if (s.nameType == exception) if (s.source) s.source.Play();
        foreach (AudioSource aS in _externalGOList) aS.gameObject.SetActive(true);
        foreach (AudioSource aS in _externalGOList) aS.Play();
    }
    public void ResumeAllButNoMusic(object[] obj)
    {
        PlayAll();
        _pause = false;
        //SoundsTypes exception = SoundsTypes.Music;
        //if (obj[0] != null && (SoundsTypes)obj[0] != exception) exception = (SoundsTypes)obj[0];
        //foreach (Sound s in sounds) if (s.nameType == exception) if (s.source) s.source.Pause();
        //foreach (AudioSource aS in _externalGOList) aS.gameObject.SetActive(true);
        //foreach (AudioSource aS in _externalGOList) aS.Play();
    }

    public void Pause(SoundsTypes name, GameObject request = default)
    {
        Sound s = _usedSounds.ReturnValue(name);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }

        s = SoundSet(s, request);
        s.source.Pause();
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
        s.source.Pause();
    }

    public void PauseAll(SoundsTypes exception = SoundsTypes.Music)
    {
        //Debug.Log($"<color=orange>Sound: {ToString()} PauseAll function enter !</color>");
        _pause = true;

        foreach (Sound s in sounds) if (s.nameType == exception) if (s.source) s.source.Pause();
        foreach (AudioSource aS in _externalGOList) aS.Pause();
        foreach (AudioSource aS in _externalGOList) aS.gameObject.SetActive(false);
    }
    public void PauseAllButNoMusic(object[] obj)
    {
        //PauseAll();
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
        s.source.loop = false;
        s.source.Play();
    }

    private Sound SoundSet(Sound s, GameObject request = default)
    {
        if (s == null) return null;
        if (ManagerAudioSourceConfig(s)) return s;

        SoundSpawn soundObject = null;
        if (request && request != gameObject) soundObject = _searchRequest.ReturnValue(request);
        if (soundObject)
        {
            //if (!FoundEqualSound(s.source.clip, request)) soundObject.sources.Add(s.source);
            if (request.GetComponentInChildren<AudioSource>()) if (FoundEqualSound(s.clip, request)) return s;

            s.source = soundObject.gameObject.AddComponent<AudioSource>();
            if (!_found) soundObject.SetFather(request);
            _externalGOList.Add(s.source);
            soundObject.PauseMyself();
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
                if(!s.source) s.source = gameObject.GetComponent<AudioSource>();
            }
        }
        else s.source = gameObject.AddComponent<AudioSource>();

        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = s.audioMixerGroup;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        return s;
    }

    private bool ManagerAudioSourceConfig(Sound s)
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
                    source.clip = s.clip;
                    source.outputAudioMixerGroup = s.audioMixerGroup;
                    source.volume = s.volume;
                    source.pitch = s.pitch;
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
    private SoundSpawn SearchSoundSpawn(GameObject requester)
    {
        if (!requester) return null;
        SoundSpawn spawn = default;
        if (_pool.GetStock.Count > 0) spawn = Array.Find(_pool.GetStock.ToArray(), stock => stock.GetComponent<SoundSpawn>().Father == requester).GetComponent<SoundSpawn>();
        _found = spawn ? true : false;
        return spawn ? spawn : _pool.GetObject().GetComponent<SoundSpawn>();
    }

    private bool FoundEqualSound(AudioClip clip, GameObject target)
    {
        AudioSource[] audioSources = target.GetComponentsInChildren<AudioSource>();
        AudioClip[] audioClips = new AudioClip[audioSources.Length];
        for (int i = 0; i < audioSources.Length; audioClips[i] = audioSources[i].clip, i++)
            if (audioSources.Length <= 0) return false;
        return Array.Find(audioClips, audioClip => audioClip == clip);
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

    public void RemoveFromGOList(AudioSource obj)
    {
        _externalGOList.Remove(obj);
    }
    
    public void ResetGOList()
    {
        _externalGOList.Clear();
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