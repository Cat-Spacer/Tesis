using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSpawn : MonoBehaviour
{
    private GameObject _father = default;
    public GameObject Father { get { return _father; } }

    private AudioSource _source = default;

    [SerializeField] SoundsTypes _myType = default;
    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        SuscribeEventManager();
        if (GameManager.Instance.pause) PauseByDesactivate(default);
    }

    public void SetFather(GameObject father, SoundsTypes type)
    {
        _father = father;
        name = $"SoundSpawn {father.name}";
        gameObject.transform.parent = _father.transform;
        transform.position = _father.transform.position;
        _myType = type;
    }

    public Sound SetAudioSource(Sound s, bool loop)
    {
        if (s == null) return null;
        if (!_source) _source = GetComponent<AudioSource>();
        _source.clip = s.clip;
        _source.outputAudioMixerGroup = s.audioMixerGroup;
        _source.volume = s.volume;
        _source.pitch = s.pitch;
        _source.loop = loop;
        s.source = _source;
        return s;
    }

    public void PauseByDesactivate(object[] obj)
    {
        _source.Pause();
        gameObject.SetActive(false);
    }

    private void SuscribeEventManager()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Subscribe(EventType.OnPauseGame, PauseByDesactivate);
            EventManager.Instance.Subscribe(EventType.OnFinishGame, PauseByDesactivate);
            EventManager.Instance.Subscribe(EventType.OnLoseGame, PauseByDesactivate);
        }
    }

    public void UnsuscribeEventManager()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Unsubscribe(EventType.OnPauseGame, PauseByDesactivate);
            EventManager.Instance.Unsubscribe(EventType.OnFinishGame, PauseByDesactivate);
            EventManager.Instance.Unsubscribe(EventType.OnLoseGame, PauseByDesactivate);
        }
    }

    public void Reset()
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        if(_myType == SoundsTypes.Steps || _myType == SoundsTypes.HamsterSteps || _myType == SoundsTypes.HamsterOnTubes) _source.Pause();
        else _source.Play();
        SuscribeEventManager();
    }

    private void OnDisable()
    {
        UnsuscribeEventManager();
    }

    private void OnDestroy()
    {
        if (SoundManager.instance) SoundManager.instance.RemoveFromSoundList(this);
        UnsuscribeEventManager();
       // Debug.Log($"{name} was destroyed");
    }
}