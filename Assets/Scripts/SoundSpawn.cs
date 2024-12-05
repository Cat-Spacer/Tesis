using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSpawn : ObjectToSpawn
{
    private GameObject _father = default;
    public GameObject Father { get { return _father; } }

    private AudioSource _source = default;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        SuscribeEventManager();
        if (GameManager.Instance.pause) ReturnToStack(default);
    }

    public void SetFather(GameObject father)
    {
        _father = father;
        name = $"SoundSpawn {father.name}";
        gameObject.transform.parent = _father.transform;
        transform.position = _father.transform.position;
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

    public void AddReferences(ObjectPool<ObjectToSpawn> op)
    {
        AddReference(op);
    }

    public void ReturnToStack(object[] obj)
    {
        objectPool?.ReturnObject(this);
    }

    private void SuscribeEventManager()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Subscribe(EventType.OnPauseGame, ReturnToStack);
            EventManager.Instance.Subscribe(EventType.OnFinishGame, ReturnToStack);
        }
    }

    public void UnsuscribeEventManager()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Unsubscribe(EventType.OnPauseGame, ReturnToStack);
            EventManager.Instance.Unsubscribe(EventType.OnFinishGame, ReturnToStack);
        }
    }

    public override void Reset()
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        SuscribeEventManager();
    }

    private void OnDestroy()
    {
        if (SoundManager.instance) SoundManager.instance.RemoveFromSoundList(this);
        UnsuscribeEventManager();
        Debug.Log($"{name} was destroyed");
    }
}