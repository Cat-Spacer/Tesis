using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private Dictionary<string, Action<object[]>> _callbackDictionary = new Dictionary<string, Action<object[]>>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
            Debug.LogWarning($"Found duplicate of 'EventManager' on {gameObject.name}");
        }
    }

    public void Subscribe(string eventId, Action<object[]> callback)
    {
        if (!_callbackDictionary.ContainsKey(eventId))
            _callbackDictionary.Add(eventId, callback);
        else
        {
            _callbackDictionary[eventId] += callback;
        }
    }

    public void Unsubscribe(string eventId, Action<object[]> callback)
    {
        if (!_callbackDictionary.ContainsKey(eventId)) return;

        _callbackDictionary[eventId] -= callback;
    }

    public void Trigger(string eventId, params object[] parameters)
    {
        if (!_callbackDictionary.ContainsKey(eventId)) return;

        _callbackDictionary[eventId](parameters);
    }
}
