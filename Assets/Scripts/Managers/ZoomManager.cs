using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ZoomManager : MonoBehaviour
{
    [SerializeField] private float cameraDefaultZoom;
    [SerializeField] private float time;
    [SerializeField] private float timeToZoom;
    [SerializeField] private float speed;
    [SerializeField] private float desireZoom;
    private bool zoomIn = false;
    [SerializeField] private Button[] buttons;
    [SerializeField] private ButtonSizeUpdate[] buttonsSizeUpdate;
    [SerializeField] CinemachineVirtualCamera firstCam;
    [SerializeField] CinemachineVirtualCamera secondCam;

    private void Awake()
    {
        secondCam.m_Lens.OrthographicSize = cameraDefaultZoom;
        buttonsSizeUpdate = new ButtonSizeUpdate[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = false;
            buttonsSizeUpdate[i] = buttons[i].gameObject.GetComponent<ButtonSizeUpdate>();
            buttonsSizeUpdate[i].enabled = false;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(time);
        secondCam.Priority = 2;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeToZoom);
        zoomIn = true;
    }

    private void Update()
    {
        if (!zoomIn) return;

        var lerp = Mathf.Lerp(secondCam.m_Lens.OrthographicSize, desireZoom, Time.deltaTime * speed);
        secondCam.m_Lens.OrthographicSize = lerp;
        if (lerp < desireZoom + 0.25f)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].enabled = true;
                buttonsSizeUpdate[i].enabled = true;
            }
            zoomIn = false;
        }
    }
}