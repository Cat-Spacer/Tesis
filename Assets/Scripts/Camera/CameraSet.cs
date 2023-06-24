using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CameraSet : MonoBehaviour
{
    [SerializeField] private Camera _setCam;
    private Camera _mainCam;
    private Canvas _myCanvas;

    void Start()
    {
        if (!_mainCam) _mainCam = Camera.main;
        if (!_myCanvas) _myCanvas = GetComponent<Canvas>();
        if (FindObjectOfType<ZoomEffect>())
            if (FindObjectOfType<ZoomEffect>().GetComponent<Camera>())
                _setCam = FindObjectOfType<ZoomEffect>().GetComponent<Camera>();
    }

    void Update()
    {
        if (!_setCam)
            if (FindObjectOfType<ZoomEffect>())
                if (FindObjectOfType<ZoomEffect>().GetComponent<Camera>())
                    _setCam = FindObjectOfType<ZoomEffect>().GetComponent<Camera>();

        if (!(_setCam && _mainCam)) return;

        if (_setCam.gameObject.activeInHierarchy) _myCanvas.worldCamera = _setCam;
        else _myCanvas.worldCamera = _mainCam;
    }
}
