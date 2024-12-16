using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.PlayerLoop;

public class CameraManager : MonoBehaviour
{
    private Transform catPos, hamsterPos;
    [SerializeField] private bool isGrouped;
    [SerializeField] float cameraGroupLimit;
    [SerializeField] float cameraWarningLimit;
    [SerializeField] private float distance;
    [SerializeField] private GameObject groupCamera;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private GameObject[] splitCamera;
    [SerializeField] private GameObject tvShader;
    [SerializeField] private GameObject tvHackedShader;
    private CinemachineVirtualCamera virtualGroupCamera;
    private CinemachineVirtualCamera[] virtualSplitCamera = new CinemachineVirtualCamera[2];

    [SerializeField] SpriteRenderer warningSprite;
    private Material warningMat;
    private int Alpha = Shader.PropertyToID("_Alpha");

    private bool useCamera = true;
    
    
    
    private void Start()
    {
        catPos = GameManager.Instance.GetCat();
        hamsterPos = GameManager.Instance.GetHamster();
        warningMat = warningSprite.material;
        warningMat.SetFloat(Alpha, 0);
        LiveCamera.instance.GetTVShaders(tvShader);
        LiveCamera.instance.GetTVHackedShaders(tvHackedShader);
        EventManager.Instance.Subscribe(EventType.ShowTv, OnShowTv);
        EventManager.Instance.Subscribe(EventType.ReturnGameplay, OnResumeGameplay);
    }
    private void OnShowTv(object[] obj)
    {
        useCamera = false;
    }
    private void OnResumeGameplay(object[] obj)
    {
        useCamera = true;
        StartCoroutine(RestartCamera());
    }

    IEnumerator RestartCamera()
    {
        yield return new WaitForSecondsRealtime(1f);
        // if (isGrouped)
        // {
        //     foreach (var virtualCamera in splitCamera)
        //     {
        //         virtualCamera.gameObject.SetActive(true);
        //     }
        //     groupCamera.gameObject.SetActive(false);
        // }
        // else
        // {
        //     groupCamera.gameObject.SetActive(true);
        //     foreach (var virtualCamera in splitCamera)
        //     {
        //         virtualCamera.transform.position = virtualGroupCamera.transform.position;
        //         virtualCamera.gameObject.SetActive(false);
        //     }
        // }
    }
    private void Update()
    {
        // if (!useCamera) return;
        // distance = (catPos.position - hamsterPos.position).magnitude;
        // if (distance > cameraWarningLimit && distance < cameraGroupLimit)
        // {
        //     float normalizedValue = Normalize(distance, cameraWarningLimit, cameraGroupLimit);
        //     warningMat.SetFloat(Alpha, normalizedValue);
        // }
        // else warningMat.SetFloat(Alpha, 0);
        // if (distance >= cameraGroupLimit && !isGrouped) //IsGrouped
        // {
        //     isGrouped = true;
        //     foreach (var virtualCamera in splitCamera)
        //     {
        //         virtualCamera.gameObject.SetActive(true);
        //     }
        //     groupCamera.gameObject.SetActive(false);
        //     EventManager.Instance.Trigger(EventType.OnSplitCamera);
        // }
        // else if(distance < cameraGroupLimit && isGrouped) //NotGrouped
        // {
        //     isGrouped = false;
        //     groupCamera.gameObject.SetActive(true);
        //     foreach (var virtualCamera in splitCamera)
        //     {
        //         virtualCamera.transform.position = virtualGroupCamera.transform.position;
        //         virtualCamera.gameObject.SetActive(false);
        //     }
        //     EventManager.Instance.Trigger(EventType.OnGroupCamera);
        // }
    }
    float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.ShowTv, OnShowTv);
        EventManager.Instance.Unsubscribe(EventType.ReturnGameplay, OnResumeGameplay);
    }
}
