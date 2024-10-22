using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LiveCameraNetwork : NetworkBehaviour
{
    public static LiveCameraNetwork Instance;
    [SerializeField] private PeaceSystem _peace;
    private Animator _anim;

    [SerializeField] private float _timeOnAir;
    [SerializeField] private float _currentTime;
    
    [SerializeField] private bool _onAir;
    private bool _isOnAir;
    [SerializeField] private GameObject _camera;

    private Action _OnLiveAction = delegate {  };
    private bool _onLive = false;
    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    private void Update()
    {
        if(_onLive) _OnLiveAction();
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        //_anim.SetTrigger("GoOffAir");
    }

    public void StartLiveCamera(bool status)
    {
        _onLive = status;
        _anim.SetTrigger("GoOffAir");
    }
    public void GoOnAir()
    {
        //Debug.Log("On Air");
        _onAir = true;
        SetLiveTime();
        _OnLiveAction = TimeOnAir;
    }
    public void GoOffAir()
    {
        //Debug.Log("Off Air");
        _camera.SetActive(false);
        _onAir = false;
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
    void SetCheckLive()
    {
        _currentTime = _peace.GetNewTime();
    }
    void SetLiveTime()
    {
        _currentTime = _timeOnAir;
    }
    public void ChangePeace(int value)
    {
        ChangePeaceRpc(value);
    }

    [Rpc(SendTo.Everyone)]
    void ChangePeaceRpc(int value)
    {
        //_peace.UpdatePeace(value);
    }
    public bool IsOnAir()
    {
        return _onAir;
    }
}
