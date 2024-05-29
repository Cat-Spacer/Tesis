using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Camera Zoom & Speed")]
    [SerializeField] private float _targetZoom = 5;
    [SerializeField] private float _normalZoom = 5, _highlightZoom = 2.5f, _moveSpeed = 5, _sizeSpeed = 5;

    [Header("Buttons")]
    [SerializeField] private GameObject[] _buttonOutZoom = default;
    [SerializeField] private GameObject[] _buttonsOptions = default;

    [SerializeField, Header("Models")] private GameObject[] _CatroAndSquix = default;

    [Header("Areas")]
    [SerializeField] private Transform _monitorArea = default;
    [SerializeField] private Transform _planetsArea = default, _spaceArea = default;

    [Header("Scene Objects")]
    [SerializeField] private GameObject _monitorScreen = default;
    [SerializeField] private GameObject _title = default;

    [SerializeField, Header("Target")] private Vector3 _targetPos = default;

    [SerializeField, Header("Canvas Areas")] private Button[] _area = default;

    private Camera _mainCamera = default;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _targetPos = _mainCamera.transform.position = new Vector3(0, 0, -10);
        _mainCamera.orthographicSize = _targetZoom = _normalZoom;

        foreach (var item in _buttonOutZoom)
        {
            if (item) item.SetActive(false);
        }

        foreach (var item in _buttonsOptions)
        {
            if (item) item.SetActive(false);
        }

        foreach (var item in _CatroAndSquix)
        {
            if (item) item.SetActive(false);
        }

        if (_CatroAndSquix.Length > 0) if (_CatroAndSquix[0]) _CatroAndSquix[0].SetActive(true);
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        // OutZoom();

        if (_mainCamera != null && _targetPos != null && _mainCamera.transform.position != _targetPos)
        {
            Vector3 newPosition = Vector3.Lerp(_mainCamera.transform.position, _targetPos, _moveSpeed * Time.deltaTime);

            // Asignar la nueva posición a la cámara
            _mainCamera.transform.position = newPosition;
        }

        if (_mainCamera != null && _mainCamera.orthographicSize != _targetZoom)
        {
            float newSize = Mathf.Lerp(_mainCamera.orthographicSize, _targetZoom, _sizeSpeed * Time.deltaTime);

            // Asignar el nuevo valor al orthographic size de la cámara
            _mainCamera.orthographicSize = newSize;
        }
    }

    #region Set Objects
    public void SetCatroAndSquix(int img_arg)
    {
        if (_CatroAndSquix.Length > 0) if (_CatroAndSquix[0]) _CatroAndSquix[0].SetActive(false);
        if (_CatroAndSquix.Length > img_arg) if (_CatroAndSquix[img_arg]) _CatroAndSquix[img_arg].SetActive(true);
        foreach (var item in _area)
        {
            if (item) item.gameObject.SetActive(false);
        }
    }

    public void SetIdle()
    {
        foreach (var item in _CatroAndSquix)
        {
            if (item) item.SetActive(false);
        }
        foreach (var item in _area)
        {
            if (item) item.gameObject.SetActive(true);
        }

        if (_CatroAndSquix.Length > 0) if (_CatroAndSquix[0]) _CatroAndSquix[0].SetActive(true);
    }
    #endregion

    #region Zoom Methods
    public void ZoomMonitor()
    {
        if (_monitorScreen) _monitorScreen.SetActive(false);
        _targetPos = new Vector3(_monitorArea.position.x, _monitorArea.position.y, -10);
        _targetZoom = _highlightZoom;

        foreach (var item in _buttonOutZoom)
        {
            if (item) item.SetActive(true);
        }
        foreach (var item in _buttonsOptions)
        {
            if (item) item.SetActive(true);
        }
    }

    public void ZoomPlanets()
    {
        _targetPos = new Vector3(_planetsArea.position.x, _planetsArea.position.y, -10);
        _targetZoom = _highlightZoom;
        foreach (var item in _buttonOutZoom)
        {
            if (item) item.SetActive(true);
        }
        foreach (var item in _buttonsOptions)
        {
            if (item) item.SetActive(true);
        }
    }

    public void ZoomSpace()
    {
        foreach (var item in _buttonOutZoom) if (!item) return;
        foreach (var item in _buttonsOptions) if (!item) return;

        _title.SetActive(false);
        _targetPos = new Vector3(_spaceArea.position.x, _spaceArea.position.y, -10);
        _targetZoom = _highlightZoom;

        foreach (var item in _buttonOutZoom)
        {
            if (item) item.SetActive(true);
        }
        foreach (var item in _buttonsOptions)
        {
            if (item) item.SetActive(true);
        }
    }

    public void OutZoom()
    {
        //  _mainCamera.transform.position = new Vector3(0, 0, -10);
        if (_title) _title.SetActive(true);
        _targetPos = new Vector3(0, 0, -10);
        _targetZoom = _normalZoom;

        foreach (var item in _buttonOutZoom)
        {
            if (item) item.SetActive(false);
        }
        foreach (var item in _buttonsOptions)
        {
            if (item) item.SetActive(false);
        }

        if (_monitorScreen) _monitorScreen.SetActive(true);
    }
    #endregion
}