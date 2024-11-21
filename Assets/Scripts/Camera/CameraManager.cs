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
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private GameObject[] splitCamera;
    [SerializeField] private GameObject[] tvShader;
    [SerializeField] private GameObject[] tvHackedShader;
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
        targetGroup.AddMember(catPos, 1, 2);
        targetGroup.AddMember(hamsterPos, 1, 2);
        virtualSplitCamera[0].Follow = catPos;
        virtualSplitCamera[1].Follow = hamsterPos;
        LiveCamera.instance.GetTVShaders(tvShader);
        LiveCamera.instance.GetTVHackedShaders(tvHackedShader);
    }

    private void Update()
    {
        var distance = catPos.position - hamsterPos.position;
        if (distance.magnitude > cameraWarningLimit && distance.magnitude < cameraGroupLimit)
        {
            float normalizedValue = Normalize(distance.magnitude, cameraWarningLimit, cameraGroupLimit);
            warningMat.SetFloat(Alpha, normalizedValue);
        }
        else warningMat.SetFloat(Alpha, 0);
        if (distance.magnitude >= cameraGroupLimit && !isGrouped) //IsGrouped
        {
            isGrouped = true;
            foreach (var virtualCamera in splitCamera)
            {
                virtualCamera.gameObject.SetActive(true);
            }
            groupCamera.gameObject.SetActive(false);

        }
        else if(distance.magnitude < cameraGroupLimit && isGrouped) //NotGrouped
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
