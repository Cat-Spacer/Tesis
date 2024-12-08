using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ZoomManager : MonoBehaviour
{
    [SerializeField]
    private float _cameraDefaultZoom = 5, _time = 0.5f, _timeToZoom = 0.25f, _speed = 7f, _desireZoom = 2f;
    
    private bool _zoomIn = false;
    [SerializeField] private Button[] _buttons = default;
    [SerializeField] private ButtonSizeUpdate[] _buttonsSizeUpdate = default;
    [SerializeField] CinemachineVirtualCamera _firstCam = default, _secondCam = default;

    private void Awake()
    {
        _secondCam.m_Lens.OrthographicSize = _cameraDefaultZoom;
        _buttonsSizeUpdate = new ButtonSizeUpdate[_buttons.Length];
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].enabled = false;
            _buttonsSizeUpdate[i] = _buttons[i].gameObject.GetComponent<ButtonSizeUpdate>();
            _buttonsSizeUpdate[i].enabled = false;
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_time);
        _secondCam.Priority = 2;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_timeToZoom);
        _zoomIn = true;
    }

    private void Update()
    {
        if (!_zoomIn) return;
        var lerp = Mathf.Lerp(_secondCam.m_Lens.OrthographicSize, _desireZoom, Time.deltaTime * _speed);
        _secondCam.m_Lens.OrthographicSize = lerp;
        if (lerp < _desireZoom + 0.25f)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].enabled = true;
                _buttonsSizeUpdate[i].enabled = true;
            }

            _zoomIn = false;
            Destroy(this);
        }
    }
}