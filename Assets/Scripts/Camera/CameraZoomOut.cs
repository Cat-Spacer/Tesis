using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private bool catEnter;
    private bool hamsterEnter;
    private void OnEnable()
    {
        if (EventManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Subscribe(EventType.ReturnGameplay, OnReturnGameplay);
    }
    private void OnReturnGameplay(object[] obj)
    {
        virtualCamera.enabled = true;
    }

    private void OnPauseGame(object[] obj)
    {
        virtualCamera.enabled = false;
    }

    void ZoomOutCamera()
    {
        virtualCamera.Priority = 11;
    }    
    void StopZoomOutCamera()
    {
        if (catEnter || hamsterEnter) return;
        virtualCamera.Priority = -1;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            catEnter = true;
            ZoomOutCamera();
        }
        else if (other.gameObject.CompareTag("Hamster"))
        {
            hamsterEnter = true;
            ZoomOutCamera();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            catEnter = false;
            StopZoomOutCamera();
        }
        else if (other.gameObject.CompareTag("Hamster"))
        {
            hamsterEnter = false;
            StopZoomOutCamera();
        }
    }
}
