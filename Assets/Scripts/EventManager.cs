using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    OnPunchPlayer,
    OnSwitchCameraType,
    OnChangePeace,
    OnUpdateEgoPoints,
    OnLoseGame,
    ViewPlayerIndicator,
    GetTVShader,
    OnStartGame,
    OffLive,
    OnLive,
    OnPauseGame,
    OnResumeGame,
    StartTimer,
    StopTimer,
    OnFinishGame,
    OnSplitCamera,
    OnGroupCamera,
    OnGetShield,
    OnPutFlower,
    OnOpenDoors
}
public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private Dictionary<EventType, Action<object[]>> _callbackDictionary = new Dictionary<EventType, Action<object[]>>();
    private void Awake()
    {
        //Instance = this;
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning($"Found duplicate of 'EventManager' on {gameObject.name}");
            Destroy(this);
        }
    }

    public void Subscribe(EventType eventId, Action<object[]> callback)
    {
        if (!_callbackDictionary.ContainsKey(eventId))
            _callbackDictionary.Add(eventId, callback);
        else
        {
            _callbackDictionary[eventId] += callback;
        }
    }

    public void Unsubscribe(EventType eventId, Action<object[]> callback)
    {
        if (!_callbackDictionary.ContainsKey(eventId)) return;

        _callbackDictionary[eventId] -= callback;
    }

    public void Trigger(EventType eventId, params object[] parameters)
    {
        if (!_callbackDictionary.ContainsKey(eventId)) return;

        _callbackDictionary[eventId](parameters);
    }
}
