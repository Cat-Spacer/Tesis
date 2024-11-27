using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZoomManager : MonoBehaviour
{
    [SerializeField] private Button _interact = default;
    [SerializeField] private Button[] _buttons = default;
    [SerializeField] private GameObject _zoomed = default;
    [SerializeField] private float _zoomMultiplayer = 1f, _smothTime = 0.25f, _minZoom = 2f, _maxZoom = 5f, _zoomSpeed = 1f;
    private float _zoom = default, _smothSpeed = default;
    private Camera _mainCamera = default;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
    private Vector3 _initialPos = default;
    private ButtonSizeUpdate _sizeUpdate;
    private void Awake()
    {
        if(!_mainCamera) _mainCamera = Camera.main;
        if(!_virtualCamera) _virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        _zoom = _mainCamera.orthographicSize;
        if (_mainCamera.orthographic) _maxZoom = _mainCamera.orthographicSize;

        foreach (var button in _buttons)
        {
            button.interactable = false;
            if (button.GetComponent<ButtonSizeUpdate>()) button.GetComponent<ButtonSizeUpdate>().enabled = false;
        }
        _initialPos = _mainCamera.transform.position;
        if (_interact) if (_interact.GetComponent<ButtonSizeUpdate>()) _sizeUpdate = _interact.GetComponent<ButtonSizeUpdate>();

    }

    public void ClickForZoom(float newZoomSpeed = 0)
    {
        if (!_zoomed || !_interact || _buttons.Length < 1) return;
        if (newZoomSpeed > 0) _smothSpeed = newZoomSpeed;
        _interact.enabled = false;

        StartCoroutine(SmothZoom(_zoomMultiplayer, true, new Vector3(_zoomed.transform.position.x, _zoomed.transform.position.y, _mainCamera.transform.position.z)));
    }

    public void ClickForBack(float newZoomSpeed = 0)
    {
        if (!_zoomed || !_interact || _buttons.Length < 1) return;
        if (newZoomSpeed > 0) _smothSpeed = newZoomSpeed;

        StartCoroutine(SmothZoom(-_zoomMultiplayer, false, _initialPos));
    }

    private IEnumerator SmothZoom(float zoomTo, bool interactable, Vector3 target)
    {
        Vector3 dir = target - _mainCamera.transform.position;
        //Debug.Log($"target pos {target}");

        if (target == _initialPos)
        {
            foreach (Button button in _buttons)
            {
                button.interactable = interactable;
                if (button.GetComponent<ButtonSizeUpdate>()) button.GetComponent<ButtonSizeUpdate>().enabled = interactable;
            }
            //_sizeUpdate.enabled = !interactable;
        }

        while (_mainCamera.transform.position != target || (_mainCamera.orthographicSize > _minZoom && _mainCamera.orthographicSize < _maxZoom))
        {
            if (Vector3.Distance(_mainCamera.transform.position, target) < 0.05f) _mainCamera.transform.position = target;
            else
            {
                _mainCamera.transform.position += (Mathf.Abs(_smothSpeed) + _zoomSpeed) * Time.deltaTime * dir;
                dir = target - _mainCamera.transform.position;
            }

            _zoom -= zoomTo;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);

            if (MathF.Abs(MathF.Round(_mainCamera.orthographicSize, 2) - _minZoom) < 0.02) _mainCamera.orthographicSize = _minZoom;
            if (MathF.Abs(MathF.Round(_mainCamera.orthographicSize, 2) - _maxZoom) < 0.02) _mainCamera.orthographicSize = _maxZoom;

            //_virtualCamera.m_Lens.OrthographicSize = _mainCamera.orthographicSize;
            _mainCamera.orthographicSize = Mathf.SmoothDamp(_mainCamera.orthographicSize, _zoom, ref _smothSpeed, _smothTime);
            yield return new WaitForEndOfFrame();
            Debug.Log($"{Vector3.Distance(_mainCamera.transform.position, target)} dist to target");
        }

        Debug.Log($"Target: {target}");
        if (target != _initialPos)
        {
            foreach (Button button in _buttons)
            {
                button.interactable = interactable;
                if (button.GetComponent<ButtonSizeUpdate>()) button.GetComponent<ButtonSizeUpdate>().enabled = interactable;
            }

            _interact.enabled = interactable;
        }
            _sizeUpdate.enabled = !interactable;
    }
}