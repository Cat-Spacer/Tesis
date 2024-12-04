using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSpawn : ObjectToSpawn
{
    private GameObject _father = default;
    public GameObject Father { get { return _father; } }

    private AudioSource _source = default;

    private void Start()
    {
        if (!_source) _source = GetComponent<AudioSource>();
    }

    public SoundSpawn SetFather(GameObject father, ObjectPool<ObjectToSpawn> op)
    {
        _father = father;
        gameObject.transform.parent = _father.transform;
        transform.position = _father.transform.position;
        AddReference(op);
        if (EventManager.Instance)
        {
            EventManager.Instance.Subscribe(EventType.OnPauseGame, ReturnToStack);
            EventManager.Instance.Subscribe(EventType.OnResumeGame, ResetEvent);
        }
        return this;
    }

    public void ReturnToStack(object[] obj)
    {
        //if (_source.clip) _source.Stop();
        if (SoundManager.instance) SoundManager.instance.RemoveFromGOList(this);
        objectPool?.ReturnObject(this);
    }

    private void ResetEvent(object[] obj)
    {
        Reset();
    }

    public override void Reset()
    {
        //if (_source.clip) _source.Play
        if (SoundManager.instance) SoundManager.instance.spawnsList.Add(this);
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Unsubscribe(EventType.OnPauseGame, ReturnToStack);
            EventManager.Instance.Unsubscribe(EventType.OnResumeGame, ResetEvent);
        }
    }

    private void OnDisable()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.Unsubscribe(EventType.OnPauseGame, ReturnToStack);
            EventManager.Instance.Unsubscribe(EventType.OnResumeGame, ResetEvent);
        }
    }
}