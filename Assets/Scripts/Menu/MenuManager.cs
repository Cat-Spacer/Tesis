using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    Camera _mainCamera;
    [SerializeField] float _normalZoom = 5;
    [SerializeField] float _highlightZoom = 2.5f;
    [SerializeField] GameObject[] _buttonOutZoom;
    [SerializeField] Transform _monitorArea;
    [SerializeField] Transform _planetsArea;
    [SerializeField] Transform _spaceArea;
    [SerializeField] GameObject[] _buttonsOptions;
    [SerializeField] GameObject _monitorScreen;
    [SerializeField] GameObject _title;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _mainCamera.transform.position = new Vector3(0, 0, -10);
        _mainCamera.orthographicSize = _normalZoom;
        foreach (var item in _buttonOutZoom)
        {
            item.SetActive(false);
        }
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(false);
        }
    }

    public void ZoomMonitor()
    {
        _monitorScreen.SetActive(false);
        _mainCamera.transform.position = new Vector3(_monitorArea.position.x, _monitorArea.position.y, -10);
        _mainCamera.orthographicSize = _highlightZoom;
       /* foreach (var item in _buttonOutZoom)
        {
            item.SetActive(true);
        }*/
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(true);
        }
    }

    public void ZoomPlanets()
    {
        _mainCamera.transform.position = new Vector3(_planetsArea.position.x, _planetsArea.position.y, -10);
        _mainCamera.orthographicSize = _highlightZoom;
       /* foreach (var item in _buttonOutZoom)
        {
            item.SetActive(true);
        }*/
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(true);
        }
    }
    public void ZoomSpace()
    {
        _title.SetActive(false);
        _mainCamera.transform.position = new Vector3(_spaceArea.position.x, _spaceArea.position.y, -10);
        _mainCamera.orthographicSize = _highlightZoom;
       /* foreach (var item in _buttonOutZoom)
        {
            item.SetActive(true);
        }*/
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OutZoom();
    }
    public void OutZoom()
    {
        _title.SetActive(true);
        _mainCamera.transform.position = new Vector3(0, 0, -10);
        _mainCamera.orthographicSize = _normalZoom;
      /*  foreach (var item in _buttonOutZoom)
        {
            item.SetActive(false);

        }*/
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(false);
        }
        _monitorScreen.SetActive(true);
    }
}