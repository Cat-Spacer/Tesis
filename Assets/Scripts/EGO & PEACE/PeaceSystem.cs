using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeaceSystem : MonoBehaviour
{
    public static PeaceSystem instance;
    [SerializeField] private int _currentPeace;
    [SerializeField] private int _minPeace;
    [SerializeField] private int _midPeace;
    [SerializeField] private int _maxPeace;
    
    [SerializeField] private int _neutralTime;
    [SerializeField] private int _timeMultiplier; //Diferencia de tiempo entre cada nivel
    [SerializeField] private int _extraTime; //Alargar el tiempo base
    
    
    public Slider slider;

    private void Start()
    {
        if (instance == null) instance = this;
        
        _currentPeace = _midPeace;
        slider.minValue = _minPeace;
        slider.maxValue = _maxPeace;
    }

    public void UpdatePeace(int peace)
    {
        if (_currentPeace == _maxPeace || _currentPeace == _minPeace) return;
        _currentPeace += peace;
        slider.value = _currentPeace;
    }

    public int GetCurrentPeace()
    {
        return _currentPeace;
    }
    
    public int GetNewTime()
    {
        int newTime = _currentPeace * _timeMultiplier + _extraTime;
        return newTime;
        // int newTime = _neutralTime;
        // if (_currentPeace == _midPeace) return newTime;
        // if (_currentPeace > _midPeace)
        // {
        //     newTime = _currentPeace * 2 + 3;
        //     return newTime;
        // }
        // else
        // {
        //
        //     return newTime;
        // }
    }
}
