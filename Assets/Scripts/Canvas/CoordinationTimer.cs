using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoordinationTimer : MonoBehaviour
{
    private TextMeshProUGUI _timerText = null;
    private Image _timerImage = null, _timerSubImage = null;
    [SerializeField] private float _fadeOutTime = 0.25f, _stayTime = 0.5f;
    [SerializeField] private KeyCode _catKey = KeyCode.Z, _hamsterKey = KeyCode.KeypadEnter;

    private float _timer = 3f;
    private const float _maxTimer = 3f;
    private bool _initialized = false;
    private Color[] _colors = null;

    private void Awake()
    {
        if (!_timerText) _timerText = GetComponentInChildren<TextMeshProUGUI>();
        _timerText.text = _timer.ToString();
        if (!_timerImage) _timerImage = GetComponentInChildren<Image>();
        if (!_timerSubImage) _timerSubImage = Array.Find(GetComponentsInChildren<Image>(), img => img != _timerImage);
        
        _colors = new[] { _timerText.color, _timerImage.color, _timerSubImage.color };
        SetVisualsActive(false);
    }

    private void Update()
    {
        CheckForInput();
    }

    private void CheckForInput()
    {
        if (!_initialized && (Input.GetKeyDown(_catKey) || Input.GetKeyDown(_hamsterKey)))
            StartCoroutine(TimerAction());
    }

    private IEnumerator TimerAction()
    {
        _initialized = true;
        
        SetVisualsActive(true);
        ResetColors();
        
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            _timerText.text = Mathf.CeilToInt(_timer).ToString();
            yield return null;
        }
        
        _timer = _maxTimer;
        yield return new WaitForSeconds(_stayTime);
        
        while (_timerImage.color.a > 0)
        {
            float alpha = Time.deltaTime / _fadeOutTime;
            
            _timerText.color = new Color(_colors[0].r, _colors[0].g, _colors[0].b, _timerText.color.a - alpha);
            _timerImage.color = new Color(_colors[1].r, _colors[1].g, _colors[1].b, _timerImage.color.a - alpha);
            _timerSubImage.color = new Color(_colors[2].r, _colors[2].g, _colors[2].b, _timerSubImage.color.a - alpha);

            yield return null;
        }
        
        ResetColors();
        SetVisualsActive(false);
        _initialized = false;
    }

    private void SetVisualsActive(bool isActive)
    {
        
        _timerText.gameObject.SetActive(isActive);
        _timerImage.gameObject.SetActive(isActive);
    }

    private void ResetColors()
    {
        _timerText.color = _colors[0];
        _timerImage.color = _colors[1];
        _timerSubImage.color = _colors[2];
    }
}
