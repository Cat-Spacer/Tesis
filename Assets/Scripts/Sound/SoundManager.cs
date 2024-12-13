using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

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
    ButtonHover,
    JumpPad,
    TimeBeep,
    TimeEndBeep
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = default;
    public Sound[] sounds = default;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = default;
    private LookUpTable<string, Sound> _usedSoundsByName = default;

    [SerializeField] private int _limit = 15;

    //[SerializeField] private SoundSpawn _prefab = default;
    private bool _flag = true;
    [SerializeField] private List<AudioSource> _soundsList = new();
    [SerializeField] private AudioMixer _mixer = default;

    private Dictionary<string, float> _mixerValue = new();

    public Dictionary<string, float> MixerValue
    {
        get => _mixerValue;
    }

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
            EventManager.Instance.Subscribe(EventType.OnResumeGame, PlaySounds);
            EventManager.Instance.Subscribe(EventType.OnStartGame, PlaySounds);
            EventManager.Instance.Subscribe(EventType.OnStartGame, PlaySounds);
            EventManager.Instance.Subscribe(EventType.OnPauseGame, PauseSounds);
            EventManager.Instance.Subscribe(EventType.OnFinishGame, PauseSounds);
            EventManager.Instance.Subscribe(EventType.OnLoseGame, PauseSounds);
            EventManager.Instance.Subscribe(EventType.OnFinishGame, (object[] obj) => _soundsList.Clear());
            EventManager.Instance.Subscribe(EventType.OnLoseGame, (object[] obj) => _soundsList.Clear());
        }

        if (SaveManager.instance)
        {
            if (SaveManager.instance.JsonSaves.LoadData().mixerNames != null)
            {
                string[] names = SaveManager.instance.JsonSaves.LoadData().mixerNames.ToArray();
                float[] volumes = SaveManager.instance.JsonSaves.LoadData().volumes.ToArray();
                for (int i = 0; i < names.Length && names.Length == volumes.Length; i++)
                {
                    _mixerValue.Add(names[i], volumes[i]);
                    _mixer.SetFloat(names[i], volumes[i]);
                }
            }
        }
        else
        {
            JsonSaves saves = new();
            if (saves.LoadData().mixerNames == null) return;
            string[] names = saves.LoadData().mixerNames.ToArray();
            float[] volumes = saves.LoadData().volumes.ToArray();
            for (int i = 0; i < names.Length && names.Length == volumes.Length; i++)
            {
                _mixerValue.Add(names[i], volumes[i]);
            }
        }
    }

    private void InitialSet()
    {
        _usedSounds = new(SearchSound);
        _usedSoundsByName = new(SearchSound);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Play(SoundsTypes nameType, GameObject request = default, bool loop = false)
    {
        Sound s = SearchForRandomSound(nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {nameType} not found!</color>");
            return;
        }

        s = SoundSet(s, request, loop);
        if (s.source)
            if (s.source.gameObject.activeSelf)
                s.source.Play();
    }

    public void Play(string name, GameObject request = default, bool loop = false)
    {
        Sound s = SearchForRandomSound(_usedSoundsByName.ReturnValue(name).nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {name} not found!</color>");
            return;
        }

        s = SoundSet(s, request, loop);
        if (s.source)
            if (s.source.gameObject.activeSelf)
                s.source.Play();
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
        if (s.source)
            if (s.source.gameObject.activeSelf)
                s.source.Play();
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
        if (s.source)
            if (s.source.gameObject.activeSelf)
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
        if (s.source)
            if (s.source.gameObject.activeSelf)
                s.source.Pause();
    }

    public void PauseAll()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource audioSource in audioSources) audioSource.Pause();
    }

    private Sound SoundSet(Sound s, GameObject request = null, bool loop = false)
    {
        if (s == null) return null;

        // SoundSpawn soundObject = null;
        if (request && request != gameObject)
        {
            if (FoundEqualSound(s.clip, request))
                return s;

            s.source = request.AddComponent<AudioSource>();
            AddToSoundList(s.source);

            // SoundSpawn soundSpawn = Instantiate(_prefab);
            //
            // if (soundSpawn && soundSpawn.TryGetComponent<SoundSpawn>(out var spaw))
            // {
            //     if (spaw)
            //         soundObject = spaw;
            //     else
            //         Debug.LogWarning($"<color=orange>Sound Spawn not found!</color>");
            // }
            // else
            // {
            //     Debug.LogWarning($"<color=orange>Pool not found!</color>");
            // }
        }
        else if (ManagerAudioSourceConfig(s, loop)) return s;

        // if (soundObject)
        // {
        //     AddToSoundList(soundObject);
        //     soundObject.SetFather(request, s.nameType);
        //     return soundObject.SetAudioSource(s, loop);
        // }
        // else
        if (s.nameType == SoundsTypes.Music)
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

        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = s.audioMixerGroup;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = loop;

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
        if (!target.GetComponent<AudioSource>()) return false;
        AudioSource audioSource = target.GetComponent<AudioSource>();
        return audioSource.clip == clip;
    }

    public Sound SearchForRandomSound(SoundsTypes nameType)
    {
        LinkedList<Sound> repeats = new();
        foreach (var item in sounds)
            if (nameType == item.nameType)
                repeats.Add(item);

        Sound s = default;
        if (repeats.Count > 1)
            s = repeats[UnityEngine.Random.Range(0, repeats.Count)];
        else
            s = _usedSounds.ReturnValue(nameType);
        return s;
    }

    public void RemoveFromSoundList(AudioSource sound)
    {
        if (_soundsList != null)
            if (_soundsList.Count > 0)
                if (_soundsList.Contains(sound))
                    _soundsList.Remove(sound);
    }

    public void AddToSoundList(AudioSource sound)
    {
        if (!_soundsList.Contains(sound)) _soundsList.Add(sound);
    }

    private void PlaySounds(object[] obj)
    {
        foreach (var sound in _soundsList)
            if (!sound.isPlaying)
                sound.Play();
    }

    private void PauseSounds(object[] obj)
    {
        foreach (var sound in _soundsList)
            if (sound.isPlaying)
                sound.Pause();
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

    public void SetMixerVolume(string mixerName, float volume)
    {
        if (mixerName == null && !_mixer) return;

        if (_mixer.FindMatchingGroups(mixerName).Length < 1)
        {
            Debug.LogWarning($"<color=orange>Mixer not found!</color>");
            return;
        }

        _mixerValue[mixerName] = volume;
        if (SaveManager.instance)
        {
            List<string> mixersNames = SaveManager.instance.JsonSaves.LoadData().mixerNames;
            List<float> volumes = SaveManager.instance.JsonSaves.LoadData().volumes;
            if (!mixersNames.Contains(mixerName)) mixersNames.Add(mixerName);
            if (mixersNames.Count != volumes.Count)
            {
                int adds = mixersNames.Count - volumes.Count;

                for (int i = 0; i < adds; volumes.Add(default), i++) ;
            }

            for (int i = 0; i < mixersNames.Count; i++)
            {
                if (mixersNames[i] == mixerName) volumes[i] = volume;
            }

            SaveManager.instance.JsonSaves.SaveJson();
        }
        else
        {
            JsonSaves save = new();
            save.LoadData().mixerNames.Add(mixerName);
            save.LoadData().volumes.Add(volume);
            save.SaveJson();
        }
    }
}