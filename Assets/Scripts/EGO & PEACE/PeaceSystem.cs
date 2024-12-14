using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeaceSystem : MonoBehaviour
{
    public static PeaceSystem instance;
    [SerializeField] private Image _nullFace;
    [SerializeField] private Image[] _angryFace;
    [SerializeField] private Image[] _happyFace;
    [SerializeField] int _currentFaceLevel = 0;
    int _maxAngryFaceLevel = -3;
    int _maxHappyFaceLevel = 3;

    private void Start()
    {
        if (instance == null) instance = this;

        _angryFace = LiveCamera.instance.GetStrikesUI();
        _happyFace = LiveCamera.instance.GetShieldStrikesUI();
        if (_currentFaceLevel == 0)
            _nullFace.gameObject.SetActive(true);
        foreach (var strikes in _angryFace) strikes.gameObject.SetActive(false);
        foreach (var shield in _happyFace) shield.gameObject.SetActive(false);
        
        EventManager.Instance.Subscribe(EventType.OnChangePeace, LosePeace);
        EventManager.Instance.Subscribe(EventType.OnGetShield, SumPeace);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
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
    }
    private void LosePeace(object[] obj)
    {
        //  if (_currentAngryLevel == 3) return;
        if (_currentFaceLevel == _maxAngryFaceLevel)
            return;

        _currentFaceLevel--;

        UpdateFace();

        if (_currentFaceLevel == _maxAngryFaceLevel)
        {
            EventManager.Instance.Trigger(EventType.OnLoseGame);
        }
    }
    private void SumPeace(object[] obj)
    {
        if (_currentFaceLevel == _maxHappyFaceLevel)
            return;

        _currentFaceLevel++;
        UpdateFace();
    }

    void UpdateFace()
    {
        if (_currentFaceLevel != 0)
            _nullFace.gameObject.SetActive(false);

        if (_currentFaceLevel == 0)
        {
            _nullFace.gameObject.SetActive(true);
            foreach (var item in _angryFace)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in _happyFace)
            {
                item.gameObject.SetActive(false);
            }
        }
        else if (_currentFaceLevel < 0)
        {
            var value = _currentFaceLevel * -1;
            for (int i = 0; i < _angryFace.Length; i++)
            {
                if (i == value-1)
                    _angryFace[i].gameObject.SetActive(true);
                else
                    _angryFace[i].gameObject.SetActive(false);
            }
        }
        else if (_currentFaceLevel > 0)
        {
            for (int i = 0; i < _happyFace.Length; i++)
            {
                if (i == _currentFaceLevel-1)
                    _happyFace[i].gameObject.SetActive(true);
                else
                    _happyFace[i].gameObject.SetActive(false);
            }
        }

    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnChangePeace, LosePeace);
        EventManager.Instance.Unsubscribe(EventType.OnGetShield, SumPeace);
    }
}
