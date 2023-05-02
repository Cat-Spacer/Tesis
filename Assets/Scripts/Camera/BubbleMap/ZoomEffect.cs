using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomEffect : MonoBehaviour
{
    [SerializeField] private float _zoom = 2f;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Rect _wantedRect = new Rect();
    private CircleCollider2D _circleCollider2D;
    private BoxCollider2D _boxCollider2D;
    private Rect _orgRect = new Rect();
    private Vector3 _orgScale = Vector3.one;

    private void Start()
    {
        if (!_rectTransform)
            _rectTransform = GetComponent<RectTransform>();
        if (!_camera)
            _camera = GetComponent<Camera>();
        if (_camera)
            _orgRect = _camera.rect;
        if (!_circleCollider2D && !_camera)
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            if (_rectTransform && _circleCollider2D)
            {
                _orgScale = _rectTransform.localScale;
                _circleCollider2D.radius = _rectTransform.rect.width / 2.0f;
            }
        }
        if (!_boxCollider2D && !_camera)
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            if (_rectTransform && _boxCollider2D)
            {
                _orgScale = _rectTransform.localScale;
                _boxCollider2D.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
            }
        }
    }

    private void Update()
    {
        if (!_camera) return;
        if (Input.mousePosition.x > _camera.pixelHeight / 2 && Input.mousePosition.y < _camera.pixelWidth / 2)
        {
            //Debug.Log("MouseOverScreen");
            _camera.rect = _wantedRect;
        }
        else
            _camera.rect = _orgRect;
    }

    private void OnMouseOver()
    {
        if (!_rectTransform) return;
        _rectTransform.localScale = _orgScale * _zoom;
    }

    private void OnMouseExit()
    {
        if (!_rectTransform) return;
        _rectTransform.localScale = _orgScale;
    }
}
