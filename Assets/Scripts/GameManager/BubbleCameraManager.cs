using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCameraManager : MonoBehaviour
{
    [SerializeField] FadeInOut _fadeInOut;
    [SerializeField] ZoomEffect _zoomEffect;
    [SerializeField] Hamster _hamster;

    Camera _camera;
    Plane[] _cameraFrustum;
    Collider2D _col;

    void Start()
    {
        _fadeInOut = FindObjectOfType<FadeInOut>();
        _zoomEffect = FindObjectOfType<ZoomEffect>();
        _hamster = FindObjectOfType<Hamster>();
        _camera = Camera.main;
        _col = _hamster.gameObject.GetComponent<Collider2D>();
        _zoomEffect.gameObject.SetActive(false);

    }

    void Update()
    {
        CameraInput();
    }

    private void CameraInput()
    {
        if (!(_fadeInOut && _zoomEffect && _hamster)) return;
        var bounds = _col.bounds;
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);

        if (GeometryUtility.TestPlanesAABB(_cameraFrustum, bounds))
        {
            _hamster.visible = true;
            _zoomEffect.transform.gameObject.SetActive(false);
            _fadeInOut.StartFades(false);
        }
        else
        {
            _hamster.visible = false;
            _zoomEffect.transform.gameObject.SetActive(true);
            _fadeInOut.StartFades(true);
        }
    }
}