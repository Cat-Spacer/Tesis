using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    TimeEndBeep,
    Hacking,
    Error,
    Magic,
    GateOpen,
    GateClose
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public Sound[] sounds = null;
    private LookUpTable<SoundsTypes, Sound> _usedSounds = null;
    private LookUpTable<string, Sound> _usedSoundsByName = null;

    [SerializeField] private int _limit = 15;
    [SerializeField] private float _fadeTimer = 0.25f, _spatialBlend = 0.5f, _minDistance = 9.0f;

    private bool _flag = true;
    [SerializeField] private AudioMixer _mixer = null;

    private Dictionary<string, float> _mixerValue = new();

    public Dictionary<string, float> MixerValue => _mixerValue;

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
        SaveInstance();
    }

    private void InitialSet()
    {
        _usedSounds = new(SearchSound);
        _usedSoundsByName = new(SearchSound);
    }

    private void SaveInstance()
    {
        if (SaveManager.instance)
        {
            if (SaveManager.instance.JsonSaves.LoadData().mixerNames == null) return;
            string[] names = SaveManager.instance.JsonSaves.LoadData().mixerNames.ToArray();
            float[] volumes = SaveManager.instance.JsonSaves.LoadData().volumes.ToArray();
            for (int i = 0; i < names.Length && names.Length == volumes.Length; i++)
            {
                _mixerValue.Add(names[i], volumes[i]);
                _mixer.SetFloat(names[i], volumes[i]);
            }
        }
        else
        {
            JsonSaves saves = new();
            if (saves.LoadData().mixerNames == null) return;
            string[] names = saves.LoadData().mixerNames.ToArray();
            float[] volumes = saves.LoadData().volumes.ToArray();
            for (int i = 0;
                 i < names.Length && names.Length == volumes.Length;
                 _mixerValue.Add(names[i], volumes[i]), i++) ;
        }
    }

    public void Play(SoundsTypes nameType, GameObject request = null, bool loop = false)
    {
        Sound s = SearchForRandomSound(nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {nameType} not found!</color>");
            return;
        }

        s = SoundSet(s, request, loop);

        if (!s.source) return;

        if (s.source.gameObject.activeSelf)
        {
            s.source.Play();
            if (!s.source.loop && request) StartCoroutine(FadeOut(s, s.clip.length + _fadeTimer));
        }

        if (!request || !GameManager.Instance) return;
        if (GameManager.Instance.pause) s.source.Pause();
    }

    public void Play(string soundName, GameObject request = null, bool loop = false)
    {
        Sound s = SearchForRandomSound(_usedSoundsByName.ReturnValue(soundName).nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {soundName} not found!</color>");
            return;
        }

        s = SoundSet(s, request, loop);
        if (!s.source) return;
        if (s.source.gameObject.activeSelf)
            s.source.Play();
        if (!request || !GameManager.Instance) return;
        if (GameManager.Instance.pause)
            s.source.Pause();
    }

    public void OnClickSound(string soundName)
    {
        Sound s = SearchForRandomSound(_usedSoundsByName.ReturnValue(soundName).nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {soundName} not found!</color>");
            return;
        }

        s = SoundSet(s);
        if (!s.source) return;
        if (s.source.gameObject.activeSelf)
            s.source.Play();
    }

    public void Pause(SoundsTypes nameType, GameObject request = null)
    {
        Sound s = _usedSounds.ReturnValue(nameType);
        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {nameType} not found!</color>");
            return;
        }

        s = SoundSet(s, request);
        if (!s.source) return;
        if (s.source.gameObject.activeSelf && s.source.isPlaying && !s.source.loop)
            StartCoroutine(FadeOut(s, _fadeTimer));
        else s.source.Pause();
    }

    public void Pause(string soundName)
    {
        Sound s = _usedSoundsByName.ReturnValue(soundName);

        if (s == null)
        {
            Debug.LogWarning($"<color=yellow>Sound: {soundName} not found!</color>");
            return;
        }

        s = SoundSet(s);
        if (!s.source) return;
        if (s.source.gameObject.activeSelf && s.source.isPlaying)
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
        Sound aux = null;
        if (request && request != gameObject)
        {
            AudioSource audioSource = FoundEqualSound(s.clip, request);
            if (audioSource)
            {
                s.source = audioSource;
                return s;
            }

            s.source = !request.GetComponent<AudioSource>()
                ? request.AddComponent<AudioSource>()
                : Array.Find(request.GetComponents<AudioSource>(), source => source.loop == loop);
            if(!s.source) s.source = request.AddComponent<AudioSource>();
        }
        else
        {
            aux = ManagerAudioSourceConfig(s, loop);
            if (aux != null) return aux;
        }

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
        else if (!s.source && !request) s.source = gameObject.AddComponent<AudioSource>();

        if (s.source)
        {
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            if (s.nameType == SoundsTypes.Music && SceneManager.GetActiveScene().buildIndex > 3)
                s.source.volume = s.volume * 0.5f;
            else s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = loop;
            s.source.playOnAwake = s.source.gameObject.CompareTag("Untagged");
            if (!request) return s;
            s.source.spatialBlend = _spatialBlend;
            s.source.minDistance = _minDistance;
            return s;
        }

        Debug.LogWarning($"<color=orange>Source not found!</color>");
        return s;
    }

    private Sound ManagerAudioSourceConfig(Sound s, bool loop, int limit = 0)
    {
        AudioSource[] allSourcess = GetComponents<AudioSource>();
        int count = 0;
        foreach (AudioSource source in allSourcess)
        {
            if (source.clip)
                if (source.clip == s.clip)
                    return s;
            if (source.outputAudioMixerGroup != s.audioMixerGroup) continue;

            count++;
            if (count != _limit) continue;

            s.source = source;
            source.clip = s.clip;
            source.outputAudioMixerGroup = s.audioMixerGroup;
            source.volume = s.volume;
            source.pitch = s.pitch;
            source.loop = loop;

            return s;
        }

        return null;
    }

    private Sound SearchSound(SoundsTypes nameType)
    {
        return Array.Find(sounds, sound => sound.nameType == nameType);
    }

    private Sound SearchSound(string soundName)
    {
        return Array.Find(sounds, sound => sound.name == soundName);
    }

    private static AudioSource FoundEqualSound(AudioClip clip, GameObject target)
    {
        if (!target.GetComponent<AudioSource>()) return null;
        AudioSource[] audioSources = target.GetComponents<AudioSource>();
        return Array.Find(audioSources, sound => sound.clip == clip);
    }

    private Sound SearchForRandomSound(SoundsTypes nameType)
    {
        LinkedList<Sound> repeats = new();
        foreach (var item in sounds)
            if (nameType == item.nameType)
                repeats.Add(item);

        Sound s = null;
        s = repeats.Count > 1 ? repeats[UnityEngine.Random.Range(0, repeats.Count)] : _usedSounds.ReturnValue(nameType);
        return s;
    }

    private static IEnumerator FadeOut(Sound sound, float fadeTime)
    {
        if (!sound.source) yield break;
        while (sound.source.volume > 0)
        {
            if (!sound.source) yield break;
            sound.source.volume -= sound.volume * Time.deltaTime / fadeTime;

            if (!sound.source) yield break;
            yield return null;
            if (!sound.source) yield break;
        }

        if (!sound.source) yield break;
        sound.source.volume = sound.volume;
        sound.source.Stop();
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
            if (mixersNames.Count - volumes.Count > 0)
                for (int i = 0, adds = mixersNames.Count - volumes.Count; i < adds; volumes.Add(1), i++)
                    ;

            for (int i = 0; i < mixersNames.Count; i++)
                if (mixersNames[i] == mixerName)
                    SaveManager.instance.JsonSaves.LoadData().volumes[i] = volume;

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