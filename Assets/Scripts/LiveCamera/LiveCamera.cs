using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveCamera : MonoBehaviour
{
    public static LiveCamera instance;
    [SerializeField] private PeaceSystem _peace;
    private Animator _anim;

    [SerializeField] private float _timeOnAir;
    [SerializeField] private float _currentTime;
    
    [SerializeField] private bool _onAir;
    private bool _isOnAir;
    [SerializeField] private GameObject _camera;

    private Action _OnLiveAction = delegate {  };
    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    private void Update()
    {
        _OnLiveAction();
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.SetTrigger("GoOffAir");
    }
    
    public void GoOnAir()
    {
        Debug.Log("On Air");
        _onAir = true;
        SetLiveTime();
        _OnLiveAction = TimeOnAir;
    }
    public void GoOffAir()
    {
        //Debug.Log("Off Air");
        _onAir = false;
        _camera.SetActive(false);
        SetCheckLive();
        _OnLiveAction = TimeUntilGoOnAir;
    }
    void TimeUntilGoOnAir()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            _camera.SetActive(true);
            _anim.SetTrigger("GoOnAir");
            _OnLiveAction = delegate {  };
        }
    }
    void TimeOnAir()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            _anim.SetTrigger("GoOffAir");
            _OnLiveAction = delegate {  };
        }
    }
    public void SetCheckLive()
    {
        _currentTime = _peace.GetNewTime();
    }
    public void SetLiveTime()
    {
        _currentTime = _timeOnAir;
    }
    public void ChangePeace(int value)
    {
        _peace.UpdatePeace(value);
    }
    public bool IsOnAir()
    {
        return _onAir;
    }
}
