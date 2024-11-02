using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.PlayerLoop;

public class CameraManager : MonoBehaviour
{
    private Transform catPos, hamsterPos;
    [SerializeField] private bool isGrouped;
    [SerializeField] float cameraGroupLimit;
    [SerializeField] private GameObject groupCamera;
    [SerializeField] private GameObject[] splitCamera;

    private CinemachineVirtualCamera virtualGroupCamera;
    private CinemachineVirtualCamera[] virtualSplitCamera = new CinemachineVirtualCamera[2];
    
    
    private void Start()
    {
        catPos = GameManager.Instance.GetCat();
        hamsterPos = GameManager.Instance.GetHamster();
        virtualGroupCamera = groupCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        virtualSplitCamera[0] = splitCamera[0].GetComponentInChildren<CinemachineVirtualCamera>();
        virtualSplitCamera[1] = splitCamera[1].GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        var distance = catPos.position - hamsterPos.position;
        if (distance.magnitude >= cameraGroupLimit && !isGrouped)
        {
            isGrouped = true;
            foreach (var virtualCamera in splitCamera)
            {
                //virtualCamera.transform.position = virtualGroupCamera.transform.position;
                virtualCamera.gameObject.SetActive(true);
            }
            groupCamera.gameObject.SetActive(false);
        }
        else if(distance.magnitude < cameraGroupLimit && isGrouped)
        {
            isGrouped = false;
            groupCamera.gameObject.SetActive(true);
            foreach (var virtualCamera in splitCamera)
            {
                virtualCamera.transform.position = virtualGroupCamera.transform.position;
                virtualCamera.gameObject.SetActive(false);
            }
        }
    }
}
