﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGO : IScreen
{
    private Dictionary<Behaviour, bool> _before = new ();

    private Transform _rootGame = null;
    private readonly Behaviour[] _behaviours = null;

    public ScreenGO(Transform root)
    {
        _before ??= new Dictionary<Behaviour, bool>();

        _rootGame = root;
        _behaviours = _rootGame.GetComponentsInChildren<Behaviour>();
    }

    public void Activate()
    {
        foreach (var keyValue in _before)
        {
            keyValue.Key.enabled = keyValue.Value;

            //keyValue.Key.gameObject.SetActive(true);
            if(keyValue.Key.GetComponent<Rigidbody2D>()) keyValue.Key.GetComponent<Rigidbody2D>().isKinematic = false;
        }

        _before.Clear();
    }

    public void Deactivate()
    {
        foreach (var b in _behaviours)
        {
            if (!b) continue;
            _before[b] = b.enabled;
            if(b.enabled) b.enabled = false;

            if(b.GetComponent<Rigidbody2D>()) b.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    public string Free()
    {
        //GameObject.Destroy(_rootGame.gameObject);
        return "Playable Game";
    }
}