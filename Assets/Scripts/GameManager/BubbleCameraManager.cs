using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCameraManager : MonoBehaviour
{
    [SerializeField] private FadeInOut _fadeInOut;
    [SerializeField] private ZoomEffect _zoomEffect;
    [SerializeField] private Hamster _hamster;

    private Camera _camera;
    private Collider2D _col;
    [SerializeField] private GameObject _container;
    private Plane[] _cameraFrustum;
    [SerializeField] private bool _startedCorroutine;

    void Start()
    {
        _fadeInOut = FindObjectOfType<FadeInOut>();
        _zoomEffect = FindObjectOfType<ZoomEffect>();
        _hamster = FindObjectOfType<Hamster>();
        _camera = Camera.main;
        _col = _hamster.gameObject.GetComponent<Collider2D>();
        _startedCorroutine = true;
        _container = _zoomEffect.transform.gameObject;
    }

    void LateUpdate()
    {
        CameraInput();
    }

    private void CameraInput()
    {
        if (!(_fadeInOut && _container && _hamster)) return;
        var bounds = _col.bounds;
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);

        if (GeometryUtility.TestPlanesAABB(_cameraFrustum, bounds))
        {
            if (_startedCorroutine)
            {
                Debug.Log($"_startedCorroutine = {_startedCorroutine}");
                _fadeInOut.StartFades(false, _container);
                _startedCorroutine = false;
            }
        }
        else
        {
            if (!_startedCorroutine)
            {
                Debug.Log($"_startedCorroutine = {_startedCorroutine}");

                _fadeInOut.StartFades(true, _container);
                _startedCorroutine = true;
            }
        }
    }
}