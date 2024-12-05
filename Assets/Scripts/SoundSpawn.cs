using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSpawn : ObjectToSpawn
{
    private GameObject _father = default;
    public GameObject Father { get { return _father; } }

    public void SetFather(GameObject father)
    {
        _father = father;
        name = $"SoundSpawn {father.name}";
        gameObject.transform.parent = _father.transform;
        transform.position = _father.transform.position;
    }

    public void AddReferences(ObjectPool<ObjectToSpawn> op)
    {
        AddReference(op);
        SuscribeEventManager();
        if (SoundManager.instance) SoundManager.instance.AddToSoundList(this);
        if (GameManager.Instance.pause) ReturnToStack(default);
    }

    public void ReturnToStack(object[] obj)
    {
        if (SoundManager.instance) SoundManager.instance.AddToSoundList(this);
        objectPool?.ReturnObject(this);
    }

    private void ResetEvent(object[] obj)
    {
        Reset();
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
        SuscribeEventManager();
    }

    private void OnDestroy()
    {
        if (SoundManager.instance) SoundManager.instance.RemoveFromSoundList(this);
        UnsuscribeEventManager();
        Debug.Log($"{name} was destroyed");
    }
}