using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelNameBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private bool _useAutomatic;   
    private void Awake()
    {
        if (!_useAutomatic) return;
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = gameObject.name;
    }
}
