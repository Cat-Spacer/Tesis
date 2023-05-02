using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersSize : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _orgSize = Vector2.zero, _wantedSize = Vector2.zero;
    [SerializeField] private RectTransform _rectTransform;
    private Vector3 _orgScale = Vector3.one;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _orgScale = _rectTransform.localScale;
        _orgSize = _rectTransform.sizeDelta;
        Debug.Log(_camera.scaledPixelWidth/_camera.rect.x);
    }

    private void Update()
    {
        if (!_rectTransform) return;
        //_rectTransform.localScale = new Vector3(_camera.rect.x,Mathf.Abs( _camera.rect.y)) + _orgScale;
        if (_camera.rect.x != 0.755f )
        {
            _rectTransform.sizeDelta = _wantedSize;
        }else
            _rectTransform.sizeDelta = _orgSize;
    }
}
