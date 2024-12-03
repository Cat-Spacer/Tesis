using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeaceSystem : MonoBehaviour
{
    public static PeaceSystem instance;
    [SerializeField] private Image[] _strikes;
    [SerializeField] private Image[] _shieldStrikes;
    [SerializeField] int _currentStrike = 3;
    [SerializeField] int _currentShieldStrike = -1;
    
    private void Start()
    {
        if (instance == null) instance = this;

        _strikes = LiveCamera.instance.GetStrikesUI();
        _shieldStrikes = LiveCamera.instance.GetShieldStrikesUI();
        
        foreach (var strikes in _strikes) strikes.gameObject.SetActive(true);
        foreach (var shield in _shieldStrikes) shield.gameObject.SetActive(false);
        
        EventManager.Instance.Subscribe(EventType.OnChangePeace, UpdatePeace);
        EventManager.Instance.Subscribe(EventType.OnGetShield, GetShield);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            EventManager.Instance.Trigger(EventType.OnChangePeace);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            EventManager.Instance.Trigger(EventType.OnGetShield);
        }
    }
    private void UpdatePeace(object[] obj)
    {
        if (_currentStrike <= 0) return;
        if (_currentShieldStrike > 0)
        {
            if (_currentShieldStrike == 3)
            {
                _shieldStrikes[0].gameObject.SetActive(true);
                _shieldStrikes[1].gameObject.SetActive(true);
                _shieldStrikes[2].gameObject.SetActive(false);
            }
            else if (_currentShieldStrike == 2)
            {
                _shieldStrikes[0].gameObject.SetActive(true);
                _shieldStrikes[1].gameObject.SetActive(false);
                _shieldStrikes[2].gameObject.SetActive(false);
            }
            else if(_currentShieldStrike == 1)
            {
                _shieldStrikes[0].gameObject.SetActive(false);
                _shieldStrikes[1].gameObject.SetActive(false);
                _shieldStrikes[2].gameObject.SetActive(false);
            }
            _currentShieldStrike--;
            return;
        }
        _currentStrike--;
        _strikes[_currentStrike].color = Color.gray;
        if (_currentStrike == 0)
        {
            EventManager.Instance.Trigger(EventType.OnLoseGame);
        }
    }
    private void GetShield(object[] obj)
    {
        if (_currentShieldStrike < 3)
        {
            _currentShieldStrike++;
            _shieldStrikes[_currentShieldStrike - 1].gameObject.SetActive(true);
        }
    }
}
