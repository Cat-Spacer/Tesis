using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LiveCamera : MonoBehaviour
{
    public static LiveCamera instance;
    [SerializeField] private float _maxTimeOnAir;
    [SerializeField] private float _minTimeOnAir;
    [SerializeField] private float _timeOnAir;
    [SerializeField] private float _timeUntilOnAir;
    [SerializeField] private float _timeVariation ;
    
    [SerializeField] private bool _onAir;
    private bool _isOnAir;
    
    [SerializeField] private GameObject _groupCamera;
    [SerializeField] private GameObject[] _tvShader;
    [SerializeField] private TextMeshProUGUI[] _pointsText; 
    [SerializeField] private Slider[] sliders;
    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }
    private void ChangeCameraType(object[] obj)
    {
        var state = (bool) obj[0];
        ActivateCamera();
    }
    
    public void StartLiveCamera(bool status)
    {
        ActivateCamera();
        GoOnAir();
    }

    float CalculateTimeOnAir()
    {
        int peace = PeaceSystem.instance.GetCurrentPeace();
        float newTime = Mathf.Lerp(_minTimeOnAir, _maxTimeOnAir, 1 - peace / 10f);
        float variation = newTime * _timeVariation;
        float liveTime = Random.Range(newTime - variation, newTime + variation);
        liveTime = Mathf.Clamp(liveTime, _minTimeOnAir, _maxTimeOnAir);
        return liveTime;
    }

    float CalculateTimeUntilAir()
    {
        int peace = PeaceSystem.instance.GetCurrentPeace();
        float newTime = Mathf.Lerp(_maxTimeOnAir, _minTimeOnAir, 1 - peace / 10f);
        float variation = newTime * _timeVariation;
        float liveTime = Random.Range(newTime - variation, newTime + variation);
        liveTime = Mathf.Clamp(liveTime, _minTimeOnAir, _maxTimeOnAir);
        return liveTime;
    }
    void GoOnAir()
    {
        _onAir = true;
        ActivateCamera();
        StartCoroutine(OnAirTimer(CalculateTimeOnAir()));
    }
    IEnumerator OnAirTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        GoOffAir();
    }
    public void GoOffAir()
    {
        _onAir = false;
        DesactivateAllCameras();
        StartCoroutine(TimeUntilGoOnAir(CalculateTimeUntilAir()));
    }
    
    IEnumerator TimeUntilGoOnAir(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        GoOnAir();
    }

    void ActivateCamera()
    {
        if (!_onAir) return;
        _groupCamera.SetActive(true);
        foreach (var shader in _tvShader) shader.SetActive(true);
    }

    private void DesactivateAllCameras()
    {
        _groupCamera.SetActive(false);
        foreach (var shader in _tvShader) shader.SetActive(false);
    }
    public bool IsOnAir()
    {
        return _onAir;
    }
    public void GetTVShaders(GameObject[] tvShaders)
    {
        _tvShader = tvShaders;
    }

    public TextMeshProUGUI[] GetPointsText()
    {
        return _pointsText;
    }    
    public Slider[] GetSliders()
    {
        return sliders;
    }
}
