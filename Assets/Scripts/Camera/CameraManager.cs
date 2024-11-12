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
    [SerializeField] float cameraWarningLimit;
    [SerializeField] private GameObject groupCamera;
    [SerializeField] private GameObject[] splitCamera;

    private CinemachineVirtualCamera virtualGroupCamera;
    private CinemachineVirtualCamera[] virtualSplitCamera = new CinemachineVirtualCamera[2];

    [SerializeField] SpriteRenderer warningSprite;
    private Material warningMat;
    private static readonly int Alpha = Shader.PropertyToID("_Alpha");
    private void Start()
    {
        catPos = GameManager.Instance.GetCat();
        hamsterPos = GameManager.Instance.GetHamster();
        warningMat = warningSprite.material;
        warningMat.SetFloat(Alpha, 0);
        virtualGroupCamera = groupCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        virtualSplitCamera[0] = splitCamera[0].GetComponentInChildren<CinemachineVirtualCamera>();
        virtualSplitCamera[1] = splitCamera[1].GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        var distance = catPos.position - hamsterPos.position;
        if (distance.magnitude > cameraWarningLimit && distance.magnitude < cameraGroupLimit)
        {
            float normalizedValue = Normalize(distance.magnitude, cameraWarningLimit, cameraGroupLimit);
            warningMat.SetFloat(Alpha, normalizedValue);
        }
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
    float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}
