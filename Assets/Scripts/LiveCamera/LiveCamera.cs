using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveCamera : MonoBehaviour
{
    public static LiveCamera instance;

    [SerializeField] private float _timeOnAir;
    [SerializeField] private float _timeUntilOnAir;
    
    [SerializeField] private bool _onAir;
    private bool _isOnAir;

    [SerializeField] private bool isGroupedCamera;
    [SerializeField] private GameObject _groupCamera;
    [SerializeField] private GameObject[] _splitCamera;
    
    private void Awake()
    {
        if (instance == null) 
            instance = this;

        ActivateCamera();
        GoOnAir();
    }
    private void Update()
    {

    }

    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnSwitchCameraType, ChangeCameraType);
    }

    private void ChangeCameraType(object[] obj)
    {
        var state = (bool) obj[0];
        isGroupedCamera = state;
        ActivateCamera();
    }

    public void StartLiveCamera(bool status)
    {

    }
    void GoOnAir()
    {
        _onAir = true;
        ActivateCamera();
        StartCoroutine(OnAirTimer());
    }
    IEnumerator OnAirTimer()
    {
        yield return new WaitForSecondsRealtime(_timeOnAir);
        GoOffAir();
    }
    public void GoOffAir()
    {
        //Debug.Log("Off Air");
        _onAir = false;
        DesactivateAllCameras();
        StartCoroutine(TimeUntilGoOnAir());
    }
    
    IEnumerator TimeUntilGoOnAir()
    {
        yield return new WaitForSecondsRealtime(_timeUntilOnAir);
        GoOnAir();
    }

    void ActivateCamera()
    {
        if (!_onAir) return;
        if (isGroupedCamera)
        {
            foreach (var cam in _splitCamera)
            {
                cam.SetActive(false);
            }
            _groupCamera.SetActive(true);
        }
        else
        {
            foreach (var cam in _splitCamera)
            {
                cam.SetActive(true);
            }
            _groupCamera.SetActive(false);
        }
    }

    private void DesactivateAllCameras()
    {
        foreach (var cam in _splitCamera)
        {
            cam.SetActive(false);
        }
        _groupCamera.SetActive(false);
    }
    public void ChangePeace(int value)
    {
        //_peace.UpdatePeace(value);
    }
    public bool IsOnAir()
    {
        return _onAir;
    }
}
