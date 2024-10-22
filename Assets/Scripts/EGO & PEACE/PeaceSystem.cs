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
    
    [SerializeField] Slider redSlider;
    [SerializeField] Slider greenSlider;

    private void Start()
    {
        if (instance == null) instance = this;
        
        EventManager.Instance.Subscribe(EventType.OnChangePeace, UpdatePeace);

        _midPeace = Mathf.RoundToInt(_maxPeace * .5f);
        _currentPeace = _midPeace;
        redSlider.minValue = _minPeace;
        redSlider.maxValue = _midPeace;
        greenSlider.minValue = _minPeace;
        greenSlider.maxValue = _midPeace;
        redSlider.value = 0;
        greenSlider.value = 0;
    }

    private void UpdatePeace(object[] obj)
    {
        int peace = (int) obj[0];
        if (_currentPeace <= _minPeace || _currentPeace >= _maxPeace) return;
        _currentPeace += peace;
        if (_currentPeace <= 5)
        {
            Debug.Log("Red");
            redSlider.value -= peace;
        }
        else
        {
            Debug.Log("Green");
            greenSlider.value += peace;
        }
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
