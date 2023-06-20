using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersSize : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _orgSize = Vector2.zero, _wantedSize = Vector2.zero;
    [SerializeField] private float _scale = 2.0f;
    [SerializeField] private RectTransform _rectTransform;
    private Vector3 _orgScale = Vector3.one;

    private ZoomEffect _bubbleCameraZoom;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _orgScale = _rectTransform.localScale;
        _orgSize = _rectTransform.sizeDelta;
        if (!(_bubbleCameraZoom && _camera))
            if (_camera.GetComponent<ZoomEffect>()) _bubbleCameraZoom = _camera.GetComponent<ZoomEffect>();
        if (_wantedSize == Vector2.zero) _wantedSize = _orgSize * _scale;
    }

    private void Update()
    {
        if (!(_bubbleCameraZoom && _camera && _rectTransform)) return;
        if (_camera.rect.x != _bubbleCameraZoom.orgRect.x)
        {
            _rectTransform.sizeDelta = _wantedSize;
        }
        else
            _rectTransform.sizeDelta = _orgSize;
    }
}