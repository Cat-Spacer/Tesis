using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MyCameraManager : MonoBehaviour
{
    public static MyCameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCamera;
    
    [Header("Controls for lerping the Y Damping during player Jump/fall")] 
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }
    private Coroutine _lerpYPanCoroutine;
    private CinemachineFramingTransposer _framingTransposer;
    
    private CinemachineVirtualCamera _currentCamera;

    private float _normYPanAmount;
    private void Awake()
    {
        if (instance == null) instance = this;

        for (int i = 0; i < _allVirtualCamera.Length; i++)
        {
            if (_allVirtualCamera[i].enabled)
            {
                //set the current active camera
                _currentCamera = _allVirtualCamera[i];
                
                //set the framing transposer
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        _normYPanAmount = _framingTransposer.m_YDamping;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        float elapstime = 0f;
        while (elapstime < _fallYPanTime)
        {
            elapstime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapstime / _fallYPanTime));
            _framingTransposer.m_YDamping = lerpedPanAmount;
            yield return null;
        }
        IsLerpingYDamping = false;
    }
}


