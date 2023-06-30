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
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] Vector3 _targetPos;
    [SerializeField] float _targetZoom;
    [SerializeField] float _sizeSpeed = 5;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _targetPos = new Vector3(0, 0, -10);
        _mainCamera.transform.position = new Vector3(0, 0, -10);
        _mainCamera.orthographicSize = _normalZoom;
        _targetZoom = _normalZoom;
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
        _targetPos = new Vector3(_monitorArea.position.x, _monitorArea.position.y, -10);
        _targetZoom = _highlightZoom;
       foreach (var item in _buttonOutZoom)
        {
            item.SetActive(true);
        }
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(true);
        }
    }

    public void ZoomPlanets()
    {
        _targetPos = new Vector3(_planetsArea.position.x, _planetsArea.position.y, -10);
        _targetZoom = _highlightZoom;
        foreach (var item in _buttonOutZoom)
        {
            item.SetActive(true);
        }
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(true);
        }
    }
    public void ZoomSpace()
    {
        _title.SetActive(false);
        _targetPos = new Vector3(_spaceArea.position.x, _spaceArea.position.y, -10);
        _targetZoom = _highlightZoom;
        foreach (var item in _buttonOutZoom)
        {
            item.SetActive(true);
        }
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OutZoom();

        if (_mainCamera!= null && _targetPos!=null &&_mainCamera.transform.position != _targetPos)
        {
             Vector3 newPosition = Vector3.Lerp(_mainCamera.transform.position, _targetPos, _moveSpeed * Time.deltaTime);

             // Asignar la nueva posición a la cámara
             _mainCamera.transform.position = newPosition;        
        }

        if (_mainCamera != null  && _mainCamera.orthographicSize != _targetZoom)
        {
            float newSize = Mathf.Lerp(_mainCamera.orthographicSize, _targetZoom, _sizeSpeed * Time.deltaTime);

            // Asignar el nuevo valor al orthographic size de la cámara
            _mainCamera.orthographicSize = newSize;
        }
    }
    public void OutZoom()
    {
        _title.SetActive(true);
        //  _mainCamera.transform.position = new Vector3(0, 0, -10);
        _targetPos = new Vector3(0, 0, -10);




        _targetZoom = _normalZoom;
        foreach (var item in _buttonOutZoom)
        {
            item.SetActive(false);

        }
        foreach (var item in _buttonsOptions)
        {
            item.SetActive(false);
        }
        _monitorScreen.SetActive(true);
    }
}
